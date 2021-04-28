using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        private Action actionInvoked;
        private int time;

        public InvokeStatement(Action action, Fluent fluent, Formula formula = null) : base(action, null, formula)
        {
            this.actionInvoked = actionInvoked;
            this.time = time;
        }

    }
}
