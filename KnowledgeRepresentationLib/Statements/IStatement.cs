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

        List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time);
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

        public abstract List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time);

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
