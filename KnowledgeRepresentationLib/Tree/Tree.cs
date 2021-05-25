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
            List<Node> lastLevelNodes = root.Children;
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
        /// Tworzy korzeń drzewa z czasem -1 oraz jego dzieci
        /// </summary>
        /// <param name="description"></param>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public static Node CreateRoot(IDescription description, IScenario scenario)
        {
            Node root = new Node(null, new State(new List<ActionWithTimes>(), new List<Fluent>(), new List<ActionWithTimes>(), new List<ActionWithTimes>()), -1);
            List<DataStructures.ActionWithTimes> actions = scenario.GetStartingActions(0);
            List<Observation> observations = scenario.GetObservationsAtTime(0);
            foreach(Observation observation in observations)
            {
                List<List<Fluent>> fluentsPermutations = GetAllFluentsCombinations(observation);
                foreach(List<Fluent> fluents in fluentsPermutations)
                {
                    State newState = new State(actions, fluents, new List<ActionWithTimes>(), new List<ActionWithTimes>());
                    List<State> newStates = CheckDescription(scenario, description.GetStatements(), root.CurrentState, newState, 0);
                    foreach (State state in newStates)
                    {
                        Node newNode = new Node(root, state, 0);
                        root.addChild(newNode);
                    }
                }
            }

            return root;
        }

        /// <summary>
        /// Tworzenie wszystkich kombinacji wartościowań fluentów z danej obserwacji
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public static List<List<Fluent>> GetAllFluentsCombinations(Observation observation)
        {
            List<List<Fluent>> fluentsCombinations = new List<List<Fluent>>();
            List<Fluent> fluents = observation.Form.GetFluents();
            List<List<bool>> boolCombinations = GenerateBoolCombinations(fluents.Count());
            foreach (List<bool> boolCombination in boolCombinations)
            {
                List<Fluent> fluentsCombination = new List<Fluent>();
                for (int i=0; i<fluents.Count; i++)
                {
                    Fluent fluent = (Fluent)fluents[i].Clone();
                    fluent.State = boolCombination[i];
                    fluentsCombination.Add(fluent);
                }
                fluentsCombinations.Add(fluentsCombination);
            }

            return fluentsCombinations;
        }

        /// <summary>
        /// Tworzy listę wszystkich możliwych kombinacji wartości True/False
        /// </summary>
        /// <param name="elementsNumber"></param>
        /// <returns></returns>
        public static List<List<bool>> GenerateBoolCombinations(int elementsNumber){
            List<List<bool>> combinations = Enumerable.Range(0, (int)System.Math.Pow(2, elementsNumber)).Select(i =>
            Enumerable.Range(0, elementsNumber)
                .Select(b => ((i & (1 << b)) > 0))
                .ToList()
            ).ToList();

            return combinations;
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
            State parentState = parentNode.CurrentState;
            List<DataStructures.ActionWithTimes> newActions = GetAllActionsAtTime(scenario, parentState, time);
            State newState = new State(newActions, parentState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), parentState.ImpossibleActions, parentState.FutureActions);
            List<State> newStates = CheckDescription(scenario, description.GetStatements(), parentState, newState, time);
            List<Node> newNodes = new List<Node>();
            foreach (State state in newStates)
            {
                Node node = new Node(parentNode, state, time);
                parentNode.addChild(node);
                newNodes.Add(node);
            }

            return newNodes;
        }

        public static List<State> CheckDescription(IScenario scenario, List<IStatement> statements, State parentState, State newState, int time)
        {
            List<State> states = new List<State>();
            foreach (Statement statement in statements)
            {
                // w przypadkach gdy coś zachodzi w t+1 - bierze się stan rodzica
                if (statement is CauseStatement || statement is ReleaseStatement)
                {
                    statement.CheckStatement(parentState.CurrentActions[0], parentState.Fluents, parentState.ImpossibleActions, time - 1);
                }
                else 
                {
                    statement.CheckStatement(newState.CurrentActions[0], newState.Fluents, newState.ImpossibleActions, time);
                }
                if (statement is ReleaseStatement)
                {
                    if (states.Count == 0)
                    {
                        // rozgałęzienie - po releasie może być stary stan albo zmieniony
                        states.Add(statement.DoStatement(newState.CurrentActions, newState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), newState.ImpossibleActions, newState.FutureActions));
                    }
                    else
                    {
                        foreach (State state in states)
                        { 
                            // tworzenie rozgałęzień po releasie dla każdego z obecnych już stanów
                            states.Add(statement.DoStatement(newState.CurrentActions, newState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), newState.ImpossibleActions, newState.FutureActions));
                        }
                    }
                }
                else
                { 
                    newState = statement.DoStatement(newState.CurrentActions, newState.Fluents, newState.ImpossibleActions, newState.FutureActions);
                }
            }
            states.Add(newState);

            return states;
        }

        /// <summary>
        /// Zwraca listę wszystkich akcji, które będą trwały w danej chwili
        /// - zarówno niezakończonych z poprzedniej chwili jak i tych, które się rozpoczynają (również z listy FutureActions - ze statementów)
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="parentState"></param>
        /// <param name="time"></param>
        public static List<DataStructures.ActionWithTimes> GetAllActionsAtTime(IScenario scenario, State parentState, int time)
        {
            List<DataStructures.ActionWithTimes> actions = new List<DataStructures.ActionWithTimes>();
            foreach (DataStructures.ActionWithTimes action in parentState.CurrentActions)
            {
                if (action.GetEndTime() >= time)
                {
                    actions.Add(action);
                }
            }
            actions.AddRange(scenario.GetStartingActions(time));
            foreach (DataStructures.ActionWithTimes futureAction in parentState.FutureActions)
            {
                if (futureAction.StartTime == time)
                {
                    actions.Add(futureAction);
                }
            }

            return actions;
        }

        /// <summary>
        /// Zwraca listę wszystkich powstałych struktur na podstawie drzewa możliwości.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Lista struktur</returns>
        public static List<IStructure> GenerateStructues(Node node, IScenario scenario)
        {          
            var structure = new Structure(-1);
            var structures = new List<IStructure>() { structure };
            TreeToStructures(node, structure, structures, scenario);
            return structures;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="structure"></param>
        /// <param name="structures"></param>
        public static void TreeToStructures(Node node, Structure structure, List<IStructure> structures, IScenario scenario)
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
            var obs = scenario.GetObservationsAtTime(node.Time);
            foreach (var o in obs)
            {
                if (!o.Form.Evaluate())
                {
                    structure = new InconsistentStructure();
                    return;
                }
            }

            //dodanie elementów

            structure.TimeFluents[node.Time] = node.CurrentState.Fluents;
            if (!structure.E.Contains(curAction))
                structure.E.Add(curAction);
            structures.Add(structure);

            //koniec dodawania elementów

            if (node.Children.Count == 1)
                TreeToStructures(node.Children.FirstOrDefault(), structure, structures, scenario);
            else
            {
                foreach (Node child in node.Children)
                {
                    var newStructure = new Structure(structure);
                    structures.Add(newStructure);
                    TreeToStructures(child, newStructure, structures, scenario);
                }
            }
        }
    }
}
