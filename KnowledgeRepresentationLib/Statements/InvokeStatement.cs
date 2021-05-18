using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        private Action actionInvoked;
        //private int time; - time uwzględnić wyżej - jeżeli > to tworzymy ten statement TODO

        public InvokeStatement(Action action, Fluent fluent, Formula formula, Action actionInvoked, int time) : base(action, null, formula)
        {
            if (formula.Evaluate())
            {
                this.actionInvoked = actionInvoked;
            }
            //this.time = time;
        }

    }
}
