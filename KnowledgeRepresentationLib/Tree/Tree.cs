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
            List<Node> newNodes = new List<Node>();

            foreach (Node node in newNodes)
            {
                parentNode.addChild(node);
            }

            return newNodes;
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
