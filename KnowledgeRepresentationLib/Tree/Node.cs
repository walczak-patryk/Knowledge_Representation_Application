using System;
using System.Collections.Generic;

namespace KR_Lib.Tree
{
    public class Node
    {
        Node parent;
        List<Node> children;
        State currentState;
        
        public Node(Node parent, State currentState)
        {
            this.parent = parent;
            this.currentState = currentState;
        }

        public void addChild(Node child)
        {
            this.children.Add(child);
        }
    }
}
