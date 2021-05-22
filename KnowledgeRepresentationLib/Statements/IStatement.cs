using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System;
using System.Collections.Generic;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Statements
{
    public interface IStatement
    {
        Guid GetId();
    }
    public abstract class Statement : IStatement
    {
        public Guid guid;
        public Action action;

        protected Statement(Action action)
        {
            this.guid = Guid.NewGuid();
            this.action = action;
        }

        public abstract bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int time);

        public abstract State DoStatement(List<Action> currentActions, List<Fluent> fluents, List<Action> impossibleActions); 

        public Guid GetId()
        {
            return guid;
        }
    }
}
