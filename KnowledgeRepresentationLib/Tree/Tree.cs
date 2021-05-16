using System.Collections.Generic;
using KR_Lib.Descriptions;
<<<<<<< HEAD
using KR_Lib.Structures;
=======
>>>>>>> 367f09e6b8a815e136d6563fbb91bb29c47bb215
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
            int treeDepth = scenario.GetScenarioDuration();
            Action startAction = scenario.GetScenarios(0)[1];
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
