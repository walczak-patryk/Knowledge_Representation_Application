using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        private Action actionInvoked;
        private int time;

        public InvokeStatement(Action action, Fluent fluent, Formula formula, Action actionInvoked, int time) : base(action, null, formula)
        {
            this.actionInvoked = actionInvoked;
            this.time = time;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, int currentTime)
        {
            //chyba nie zalezy nam na aktualnej akcji, tylko wczesniejszej
            if (currentAction == action && formula.Evaluate() == true && currentTime <= currentAction.GetEndTime() + time)
            {
                return true;
            }

            return false;
        }

        public Action DoStatement(Action currentAction, List<Fluent> fluents)
        {
            return actionInvoked;
        }
    }
}
