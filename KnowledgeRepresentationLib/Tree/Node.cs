using KR_Lib.Descriptions;
using KR_Lib.Statements;
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
        
        public Node(Node parent, State currentState)
        {
            this.parent = parent;
            this.currentState = currentState;
        }

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

        public void checkDescription(List<IStatement> statements)
        {
            foreach (Statement statement in statements)
            {
                statement.CheckStatement(currentState.currentAction, currentState.Fluents, time);
                
                if(statement is CauseStatement)
                {
                    currentState.Fluents = statement.DoStatement(currentState.currentAction, currentState.Fluents);

                } else if (statement is InvokeStatement)
                {
                    currentState.currentAction = statement.DoStatement(currentState.currentAction, currentState.Fluents);

                } else if (statement is ReleaseStatement)
                {
                    currentState.Fluents = statement.DoStatement(currentState.currentAction, currentState.Fluents);

                } else if (statement is TriggerStatement)
                {
                    currentState.currentAction = statement.DoStatement(currentState.currentAction, currentState.Fluents);

                } else if (statement is ImpossibleAtStatement)
                {

                } else if (statement is ImpossibleIfStatement)
                {

                }
            }
        }
    }
}
