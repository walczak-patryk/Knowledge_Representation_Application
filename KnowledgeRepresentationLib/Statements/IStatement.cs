using KR_Lib.DataStructures;
using KR_Lib.Formulas;
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
        public Fluent fluent;
        public Formula formula;

        protected Statement(Action action, Fluent fluent = null, Formula formula = null)
        {
            this.guid = Guid.NewGuid();
            this.action = action;
            this.fluent = fluent;
            this.formula = formula;
        }

        public abstract bool CheckStatement(Action currentAction, List<Fluent> fluents, int time);

        public Guid GetId()
        {
            return guid;
        }
    }
}
