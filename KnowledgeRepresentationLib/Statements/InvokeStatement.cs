using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        private Action actionInvoked;
        private int waitTime;

        public InvokeStatement(Action action, Fluent fluent, Formula formula, Action actionInvoked, int waitTime) : base(action, null, formula)
        {
            this.actionInvoked = actionInvoked;
            this.waitTime = waitTime;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int currentTime)
        {
            return formula.Evaluate() == true && currentTime == action.GetEndTime() + waitTime && !impossibleActions.Contains(actionInvoked);
        }

        public override State DoStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions)
        {
            return new State(actionInvoked, fluents, impossibleActions);
        }
    }
}
