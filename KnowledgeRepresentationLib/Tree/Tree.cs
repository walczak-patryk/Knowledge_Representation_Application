using System.Collections.Generic;
using KR_Lib.Descriptions;
using KR_Lib.Structures;
using KR_Lib.Scenarios;
using KR_Lib.Tree;
using KR_Lib.DataStructures;

namespace KR_Lib
{
    public static class TreeMethods
    {
        /// <summary>
        /// Metoda tworząca drzewo możliwości na podstawie domeny oraz scenariusza.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="scenario"></param>
        /// <param name="fluents"></param>
        /// <returns>Korzeń powstałego drzewa możliwości.</returns>
        public static Node GenerateTree(IDescription description, IScenario scenario, List<Fluent> fluents)
        {
            List<DataStructures.Observation> reObservations;
            List<DataStructures.Action> retActions;
            (reObservations, retActions) = scenario.GetScenarios(0);
            int treeDepth = scenario.GetScenarioDuration();
            Action startAction = retActions[1];
            State startState = new State(startAction, fluents);
            Node root = new Node(null, startState);

            return root;
        }

        /// <summary>
        /// Metoda tworzy nowe liście drzewa z danego liścia-rodzica dla danego czasu
        /// </summary>
        /// <param name="time"></param>
        /// <param name="parentNode"></param>
        /// <param name="scenario"></param>
        /// <param name="description"></param>
        public static void CreateNodesAtTime(int time, Node parentNode, IScenario scenario, IDescription description)
        {

        }

        /// <summary>
        /// Metoda znajduje wszystkie stany w danym czasie na podstawie scenariusza i 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="lastState"></param>
        /// <param name="scenario"></param>
        /// <param name="description"></param>
        public static void GetAllPossibleStates(int time, State lastState, IScenario scenario, IDescription description)
        {

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
