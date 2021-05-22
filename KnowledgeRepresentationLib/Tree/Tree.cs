using System.Collections.Generic;
using KR_Lib.Descriptions;
using KR_Lib.Structures;
using KR_Lib.Scenarios;
using KR_Lib.Tree;
using KR_Lib.DataStructures;
using KR_Lib.Statements;
using System.Linq;

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
            List<DataStructures.ActionWithTimes> actions = scenario.GetStartingActions(0);
            List<Observation> observations = scenario.GetObservationsAtTime(0);
            List<Fluent> fluents = new List<Fluent>();
            List<ActionWithTimes> impossibleActions = new List<ActionWithTimes>();
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
            List<State> newStates = CheckDescription(scenario, description.GetStatements(), parentNode.CurrentState, time);
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
            List<DataStructures.ActionWithTimes> newActions = GetAllActionsAtTime(scenario, parentState, time);
            State newState = new State(newActions, parentState.Fluents, parentState.ImpossibleActions);
            foreach (Statement statement in statements)
            {
                statement.CheckStatement(newActions[0], newState.Fluents, newState.ImpossibleActions, time);
                if (statement is ReleaseStatement)
                {
                    if (states.Count == 0)
                    {
                        states.Add(newState);
                        // rozgałęzienie - po releasie może być stary stan albo zmieniony
                        states.Add(statement.DoStatement(newState.CurrentActions, newState.Fluents, newState.ImpossibleActions));
                    }
                    else
                    {
                        foreach (State state in states)
                        { 
                            // tworzenie rozgałęzień po releasie
                            states.Add(statement.DoStatement(newState.CurrentActions, parentState.Fluents, parentState.ImpossibleActions));
                        }
                    }
                }
                else
                { 
                    states.Add(statement.DoStatement(newState.CurrentActions, newState.Fluents, newState.ImpossibleActions));
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
        public static List<DataStructures.ActionWithTimes> GetAllActionsAtTime(IScenario scenario, State parentState, int time)
        {
            List<DataStructures.ActionWithTimes> actions = new List<DataStructures.ActionWithTimes>();
            foreach (DataStructures.ActionWithTimes action in parentState.CurrentActions)
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

        /// <summary>
        /// Zwraca listę wszystkich powstałych struktur na podstawie drzewa możliwości.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Lista struktur</returns>
        public static List<IStructure> GenerateStructues(Node node)
        {          
            var structure = new Structure(-1);
            var structures = new List<IStructure>() { structure };
            TreeToStructures(node, structure, structures);
            return structures;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="structure"></param>
        /// <param name="structures"></param>
        public static void TreeToStructures(Node node, Structure structure, List<IStructure> structures)
        {
            if (node == null || node.CurrentState.CurrentActions.Count > 1)
            {
                structure = new InconsistentStructure();
                return;
            }
            var curAction = node.CurrentState.CurrentActions.FirstOrDefault();
            if (curAction != null && node.CurrentState.ImpossibleActions.Contains(curAction))
            {
                structure = new InconsistentStructure();
                return;
            }

            //dodanie elementów

            structure.TimeFluents[node.Time] = node.CurrentState.Fluents;
            if (!structure.E.Contains(curAction))
                structure.E.Add(curAction);
            structures.Add(structure);

            //koniec dodawania elementów

            if (node.Children.Count == 1)
                TreeToStructures(node.Children.FirstOrDefault(), structure, structures);
            else
            {
                foreach (Node child in node.Children)
                {
                    var newStructure = new Structure(structure);
                    structures.Add(newStructure);
                    TreeToStructures(child, newStructure, structures);
                }
            }
        }
    }
}
