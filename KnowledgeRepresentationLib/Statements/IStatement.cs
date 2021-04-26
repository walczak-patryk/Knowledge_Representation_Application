using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Statements
{
    public interface IStatement
    {
        Guid Id { get; }
    }
    public abstract class Statement : IStatement
    {
        private Guid guid;
        private Action action;
        private Fluent fluent;
        private Formula formula;

        protected Statement(Action action, Fluent fluent, Formula formula = null)
        {
            this.guid = Guid.NewGuid();
            this.action = action;
            this.fluent = fluent;
            this.formula = formula;
        }
    }
}
