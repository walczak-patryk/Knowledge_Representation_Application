using System.Collections.Generic;
using KR_Lib.Descriptions;
using KR_Lib.Structures;
using KR_Lib.Scenarios;
using KR_Lib.Tree;
using KR_Lib.DataStructures;
using KR_Lib.Statements;

namespace KR_Lib
{
    public static class TreeMethods
    {
        /// <summary>
        /// Metoda tworząca drzewo możliwości na podstawie domeny oraz scenariusza.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="scenario"></param>
        /// <param name="maxTime"></param>
        /// <returns>Korzeń powstałego drzewa możliwości.</returns>
        public static Node GenerateTree(IDescription description, IScenario scenario, int maxTime)
        {
            Node root = CreateRoot(description, scenario);
            List<Node> lastLevelNodes = new List<Node>() { root };
            List<Node> nextLevelNodes = new List<Node>();
            for (int i = 1; i <= maxTime; i++)
            {
                foreach (Node node in lastLevelNodes)
                {
                    nextLevelNodes.AddRange(CreateNewNodes(description, scenario, node, i));
                }
                lastLevelNodes = nextLevelNodes;
                nextLevelNodes = new List<Node>();
            }

            return root;
        }

        /// <summary>
        /// Tworzy korzeń drzewa możliwości na podstawie domeny i scenariusza
        /// </summary>
        /// <param name="description"></param>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public static Node CreateRoot(IDescription description, IScenario scenario)
        {
            List<DataStructures.Action> actions = scenario.GetStartingActions(0);
            List<Observation> observations = scenario.GetObservationsAtTime(0);
            List<Fluent> fluents = new List<Fluent>();
            List<Action> impossibleActions = new List<Action>();
            foreach(Observation observation in observations)
            {
                fluents.AddRange(observation.Form.GetFluents());
            }
            return new Node(null, new State(actions, fluents, impossibleActions), 0);
        }

        /// <summary>
        /// Tworzy dzieci danego liścia na podstawie domeny, scenariusza i aktualnego czasu
        /// </summary>
        /// <param name="description"></param>
        /// <param name="scenario"></param>
        /// <param name="parentNode"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<Node> CreateNewNodes(IDescription description, IScenario scenario, Node parentNode, int time)
        {   
            List<State> newStates = CheckDescription(scenario, description.GetStatements(), parentNode.currentState, time);
            List<Node> newNodes = new List<Node>();
            foreach (State state in newStates)
            {
                Node node = new Node(parentNode, state, time);
                parentNode.addChild(node);
                newNodes.Add(node);
            }

            return newNodes;
        }

        public static List<State> CheckDescription(IScenario scenario, List<IStatement> statements, State parentState, int time)
        {
            List<State> states = new List<State>();
            List<DataStructures.Action> newActions = GetAllActionsAtTime(scenario, parentState, time);
            State newState = new State(newActions, parentState.Fluents, parentState.impossibleActions);
            foreach (Statement statement in statements)
            {
                statement.CheckStatement(newActions[0], newState.Fluents, newState.impossibleActions, time);
                if (statement is ReleaseStatement)
                {
                    if (states.Count == 0)
                    {
                        states.Add(newState);
                        // rozgałęzienie - po releasie może być stary stan albo zmieniony
                        states.Add(statement.DoStatement(newState.currentActions, newState.Fluents, newState.impossibleActions));
                    }
                    else
                    {
                        foreach (State state in states)
                        { 
                            // tworzenie rozgałęzień po releasie
                            states.Add(statement.DoStatement(newState.currentActions, parentState.Fluents, parentState.impossibleActions));
                        }
                    }
                }
                else
                { 
                    states.Add(statement.DoStatement(newState.currentActions, newState.Fluents, newState.impossibleActions));
                }
            }

            // jeśli nic się nie zmieniło - dodajemy stan taki sam jak u rodzica
            if (states.Count == 0)
            {
                states.Add(newState);
            }    

            return states;
        }

        /// <summary>
        /// Zwraca listę wszystkich akcji, które będą trwały w danej chwili
        /// - zarówno niezakończonych z poprzedniej chwili jak i tych, które się rozpoczynają
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="parentState"></param>
        /// <param name="time"></param>
        public static List<DataStructures.Action> GetAllActionsAtTime(IScenario scenario, State parentState, int time)
        {
            List<DataStructures.Action> actions = new List<DataStructures.Action>();
            foreach (DataStructures.Action action in parentState.currentActions)
            {
                var actionWTime = (action as ActionWithTimes);
                if (actionWTime.GetEndTime() >= time)
                {
                    actions.Add(action);
                }
            }
            actions.AddRange(scenario.GetStartingActions(time));

            return actions;
        }

        //public static GoToDeeper()

        /// <summary>
        /// Zwraca listę wszystkich powstałych struktur na podstawie drzewa możliwości.
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Lista struktur</returns>
        public static List<IStructure> GenerateStructues(Node root)
        {
            List<IStructure> structures = new List<IStructure>();
            foreach (Node child in root.children)
            {
                Structure structure = new Structure();
                structure.TimeFluents1.Add((child.time, child.currentState.Fluents));
                //structure.TimeFluents2.Add(child.time, child.currentState.Fluents);
                List<(Fluent, Action, int)> OcclusionRegions = new List<(Fluent, Action, int)>();
                foreach (var item in child.currentState.Fluents)
                    structure.OcclusionRegions.Add((item, child.currentState.currentAction, child.time));
                structure.E.Add(child.currentState.currentAction);
                structure.Acs.Add(child.currentState.currentAction);
                structures.Add(structure);
                if (child.children.Count != 0)
                    GenerateStructues(child);
            }
            return structures;
        }
    }
}
