using System;
using System.Collections.Generic;

namespace KR_Lib.Tree
{
    public class Node
    {
        Node parent;
        List<Node> children;
        State currentState;
        int time;
        
        public Node(Node parent, State currentState, int time)
        {
            this.parent = parent;
            this.currentState = currentState;
            this.time = time;
        }

        public void addChild(Node child)
        {
            this.children.Add(child);
        }
    }
}
