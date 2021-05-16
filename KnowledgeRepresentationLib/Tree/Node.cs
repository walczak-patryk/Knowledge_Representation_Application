using System;
using System.Collections.Generic;

namespace KR_Lib.Tree
{
    public class Node
    {
        Node parent;
        List<Node> children;
        State currentState;
        
        public Node()
        {
        }
    }
}
