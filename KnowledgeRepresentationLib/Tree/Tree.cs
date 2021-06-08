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
        public static Node GenerateTree(IDescription description, IScenario scenario, List<Fluent> fluents, int maxTime)
        {
            Node root = CreateRoot(description, scenario, fluents);
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
        public static Node CreateRoot(IDescription description, IScenario scenario, List<Fluent>fluents)
        {
            Node root = new Node(null, new State(new List<ActionWithTimes>(), new List<Fluent>(), new List<ActionWithTimes>(), new List<ActionWithTimes>()), -1);
            List<DataStructures.ActionWithTimes> actions = scenario.GetStartingActions(0);
            List<Observation> observations = scenario.GetObservationsAtTime(0);
            if (observations.Count == 0)
            {
                List<List<Fluent>> fluentsCombinations = GetAllFluentsCombinations(null, fluents);
                foreach (List<Fluent> fluentsCombination in fluentsCombinations)
                {
                    State newState = new State(actions, fluentsCombination, new List<ActionWithTimes>(), new List<ActionWithTimes>());
                    List<State> newStates = CheckDescription(scenario, description.GetStatements(), root.CurrentState, newState, 0);
                    foreach (State state in newStates)
                    {
                        Node newNode = new Node(root, state, 0);
                        root.addChild(newNode);
                    }
                }
            }
            else
            {
                foreach (Observation observation in observations)
                {
                    List<List<Fluent>> fluentsCombinations = GetAllFluentsCombinations(observation, fluents);
                    foreach (List<Fluent> fluentsCombination in fluentsCombinations)
                    {
                        State newState = new State(actions, fluentsCombination, new List<ActionWithTimes>(), new List<ActionWithTimes>());
                        List<State> newStates = CheckDescription(scenario, description.GetStatements(), root.CurrentState, newState, 0);
                        foreach (State state in newStates)
                        {
                            Node newNode = new Node(root, state, 0);
                            root.addChild(newNode);
                        }
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
        public static List<List<Fluent>> GetAllFluentsCombinations(Observation observation, List<Fluent> fluents)
        {
            List<List<Fluent>> fluentsCombinations = new List<List<Fluent>>();
            List<Fluent> allFluents = fluents.Select(f => (Fluent)f.Clone()).ToList();
            List<List<bool>> boolCombinations = GenerateBoolCombinations(allFluents.Count());
            foreach (List<bool> boolCombination in boolCombinations)
            {
                List<Fluent> fluentsCombination = new List<Fluent>();
                for (int i=0; i<allFluents.Count; i++)
                {
                    Fluent newFluent = (Fluent)allFluents[i].Clone();
                    newFluent.State = boolCombination[i];
                    fluentsCombination.Add(newFluent);
                }
                if (observation == null)
                {
                    fluentsCombinations.Add(fluentsCombination);
                }
                else
                {
                    observation.Formula.SetFluentsStates(fluentsCombination);
                    if (observation.Formula.Evaluate())
                        fluentsCombinations.Add(fluentsCombination);
                }
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
            List<List<(State, HashSet<Fluent>)>> statementsStates = new List<List<(State, HashSet<Fluent>)>>();
            foreach (Statement statement in statements)
            {
                var result = statement.CheckAndDo(parentState, newState, time);        
                if(result != null)
                    statementsStates.Add(result);  
            }
            if(statementsStates.Count == 0)
                return new List<State>() { newState };
            
            List<(State, HashSet<Fluent>)> listOfStates = statementsStates[0];
            for(int i = 1; i<statementsStates.Count; i++){
                var iterationState = statementsStates[i];
                var tmpList = new List<(State, HashSet<Fluent>)>();
                foreach(var f in listOfStates){
                    for(int j = 0; j<iterationState.Count; j++){
                        var statesUnion = UnionStates(f.Item1, f.Item2, iterationState[j].Item1, iterationState[j].Item2);
                        if (statesUnion.Item1 != null)
                            tmpList.Add(statesUnion);
                    }
                }
                listOfStates = tmpList;
            }

            if(listOfStates.Count == 0){
                var state = new State(parentState.CurrentActions, parentState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), parentState.ImpossibleActions, parentState.FutureActions);
                state.InvalidDescription = true;
                listOfStates.Add((state, null));
            }

            return listOfStates.Select(s => s.Item1).ToList();


            // List<State> newStates = new List<State>() { newState };
            // foreach (Statement statement in statements)
            // {
            //     statement.CheckAndDo(parentState, ref newStates, time);          
            // }
            // return newStates;
        }

        private static (State, HashSet<Fluent>) UnionStates(State state1, HashSet<Fluent> affectedFluents1, State state2, HashSet<Fluent> affectedFluents2)
        {
            var copyState = state1.Clone() as State;
            if (affectedFluents1 == null)
                affectedFluents1 = new HashSet<Fluent>();
            if (affectedFluents2 == null)
                affectedFluents2 = new HashSet<Fluent>();
            HashSet<Fluent> affectedIntersection = affectedFluents1.Intersect(affectedFluents2).ToHashSet();
            if (affectedIntersection.Any() && CheckFluentsInStatesAreValid(copyState, state2, affectedIntersection))
            {
                copyState.Union(state2);
                //tmpList.Add((copyState, affectedFluents1 && affectedFluents2));
                return (copyState, affectedFluents1.Union(affectedFluents2).ToHashSet());
            }
            else if (!affectedIntersection.Any())
            {
                copyState.Union(state2);
                var affectedUnion = affectedFluents1.Union(affectedFluents2).ToHashSet();
                foreach (var f in affectedUnion)
                {
                    var f2 = copyState.Fluents.Where(f1 => f1.Id == f.Id).SingleOrDefault();
                    if(f2!= null)
                        f2.State = f.State;
                }
                return (copyState, affectedUnion);
            }
            return (null, null);
        }
            // 1 i 2 - suma ich, 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state1"></param>
        /// <param name="state2"></param>
        /// <returns></returns>
        private static bool CheckFluentsInStatesAreValid(State state1, State state2, HashSet<Fluent> intersection){
            foreach (var f in intersection)
            {
                var s1 = state1.Fluents.Where(f1 => f1.Id == f.Id).SingleOrDefault();
                var s2 = state2.Fluents.Where(f2 => f2.Id == f.Id).SingleOrDefault();
                if (s1.State != s2.State)
                {
                    return false;
                }
            }

            return true;
            //foreach(var f1 in state1.Fluents){
            //    var s2 = state2.Fluents.Where(f2 => f2.Id == f1.Id).SingleOrDefault();
            //    if(s2 != null && s2.State != f1.State)
            //        return false;
            //}
            //return true;
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
 
                if (action.GetEndTime() > time)
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
        public static List<IStructure> GenerateStructues(Node node, IScenario scenario, int maxTime)
        {          
            var structure = new Structure(maxTime);
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
            if (node == null || node.CurrentState.InvalidDescription || node.CurrentState.CurrentActions.Count > 1)
            {
                ChangeStructureToInconsistent(structure, structures);
                return;
            }
            var curAction = node.CurrentState.CurrentActions.FirstOrDefault();
            if (curAction != null && node.CurrentState.ImpossibleActions.Contains(curAction))
            {
                var impAct = node.CurrentState.ImpossibleActions.Where(a => a == curAction).FirstOrDefault();
                if(impAct.DurationTime == -1)
                {
                    ChangeStructureToInconsistent(structure, structures);
                    return;
                }
                else if( impAct.DurationTime == curAction.DurationTime)
                {
                    ChangeStructureToInconsistent(structure, structures);
                    return;
                }             
            }
             
            var obs = scenario.GetObservationsAtTime(node.Time);
            foreach (var o in obs)
            {
                o.Formula.SetFluentsStates(node.CurrentState.Fluents);
                if (!o.Formula.Evaluate())
                {
                    ChangeStructureToInconsistent(structure, structures);
                    //structures.Remove(structure);
                    return;
                }
            }

            //dodanie elementów
            if (node.Time != -1)
            {
                structure.TimeFluents[node.Time] = node.CurrentState.Fluents;
                if (curAction != null && !structure.E.Contains(curAction))
                    structure.E.Add(curAction);
                //structures.Add(structure);
            }
            //koniec dodawania elementów

            for(int i = 0; i < node.Children.Count; i++)
            {
                if(i != 0)
                {
                    var newStructure = new Structure(structure);
                    structures.Add(newStructure);
                    TreeToStructures(node.Children[i], newStructure, structures, scenario);
                }
                else
                {
                    TreeToStructures(node.Children[i], structure, structures, scenario);
                }

            }
        }

        private static void ChangeStructureToInconsistent(IStructure structure, List<IStructure> structures)
        {
            int index = structures.IndexOf(structure);
            if (index != -1)
                structures[index] = new InconsistentStructure();
            return;
        }
    }
}
