using System.Collections.Generic;

namespace KR_Lib.Tree
{
    public class Node
    {
        private Node parent;
        public List<Node> Children { get; }
        public State CurrentState { get; }
        public int Time { get; set; }
        
        public Node(Node parent, State currentState, int time)
        {
            this.parent = parent;
            this.CurrentState = currentState;
            this.Time = time;
            Children = new List<Node>();
        }

        public void addChild(Node child)
        {
            this.Children.Add(child);
        }      
    }
}
