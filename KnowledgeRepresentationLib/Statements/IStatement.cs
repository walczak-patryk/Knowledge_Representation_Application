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

        bool GetDoFlag();
    }
    public abstract class Statement : IStatement
    {
        public Guid guid;
        public Action action;
        private bool doFlag;

        protected Statement(Action action)
        {
            this.guid = Guid.NewGuid();
            this.action = action;
        }

        

        public abstract bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time);

        public abstract List<State> DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions, int time);

        public virtual bool GetDoFlag()
        {
            return false;
        }

        public Guid GetId()
        {
            return guid;
        }
    }
}
