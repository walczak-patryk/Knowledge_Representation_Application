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
        /// <returns>Korzeń powstałego drzewa możliwości.</returns>
        public static Node GenerateTree(IDescription description, IScenario scenario)
        {
            int treeDepth = scenario.GetScenarioDuration();
            Node root = CreateRoot(description, scenario);
            List<Node> lastLevelNodes = new List<Node>() { root };
            List<Node> nextLevelNodes = new List<Node>();
            for (int i = 1; i <= treeDepth; i++)
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
            Action action = scenario.GetActionAtTime(0);
            List<Observation> observations = scenario.GetObservationsAtTime(0);
            List<Fluent> fluents = new List<Fluent>();
            foreach(Observation observation in observations)
            {
                fluents.AddRange(observation.Form.GetFluents());
            }
            return new Node(null, new State(action, fluents), 0);
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
            List<State> newStates = CheckDescription(description.GetStatements(), parentNode.currentState, time);
            List<Node> newNodes = new List<Node>();
            foreach (State state in newStates)
            {
                Node node = new Node(parentNode, state, time);
                parentNode.addChild(node);
                newNodes.Add(node);
            }

            return newNodes;
        }

        public static List<State> CheckDescription(List<IStatement> statements, State parentState, int time)
        {
            List<State> states = new List<State>();
            foreach (Statement statement in statements)
            {
                statement.CheckStatement(parentState.currentAction, parentState.Fluents, time);

                if (statement is CauseStatement)
                {
                    State newState = statement.DoStatement(parentState.currentAction, parentState.Fluents);
                    states.Add(newState);
                }
                else if (statement is InvokeStatement)
                {
                    State newState = statement.DoStatement(parentState.currentAction, parentState.Fluents);
                    states.Add(newState);
                }
                else if (statement is ReleaseStatement)
                {
                    // rozgałęzienie - po releasie może być stary stan albo zmieniony
                    State oldState = new State(parentState.currentAction, parentState.Fluents);
                    states.Add(oldState);
                    State newState = statement.DoStatement(parentState.currentAction, parentState.Fluents);
                    states.Add(newState);
                }
                else if (statement is TriggerStatement)
                {
                    State newState = statement.DoStatement(parentState.currentAction, parentState.Fluents);
                    states.Add(newState);
                }
                else if (statement is ImpossibleAtStatement)
                {

                }
                else if (statement is ImpossibleIfStatement)
                {

                }
            }

            // jeśli nic się nie zmieniło - dodajemy stan taki sam jak u rodzica
            if (states.Count == 0)
            {
                states.Add(new State(parentState.currentAction, parentState.Fluents));
            }    

            return states;
        }

        /// <summary>
        /// Zwraca listę wszystkich powstałych struktur na podstawie drzewa możliwości.
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Lista struktur</returns>
        public static List<IStructure> GenerateStructues(Node root)
        {
            return new List<IStructure>();
        }
    }
}
