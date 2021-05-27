using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        private Action actionInvoked;
        private IFormula formulaIf;
        private int waitTime;
        private bool doFlag = false;
        bool ifFlag = false;

        public InvokeStatement(ActionTime action, ActionTime actionInvoked, IFormula formulaIf = null, int waitTime = 0) : base(action)
        {
            this.actionInvoked = actionInvoked;
            if (formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }

            this.waitTime = waitTime;
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            if (action != currentAction)
            {
                doFlag = false;
                return doFlag;
            }

            bool result = true;
            int? startTime = currentAction.GetEndTime() + waitTime;
            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                result = formulaIf.Evaluate() && currentTime == startTime;
            } else
            {
                result = currentTime == startTime;
            }

            if (result)
            {
                this.actionInvoked = new ActionWithTimes(actionInvoked, (actionInvoked as ActionTime).Time, startTime.Value);
            }
            doFlag = result;
            return doFlag;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions)
        {
            if (doFlag)
            {
                futureActions.Add(actionInvoked as ActionWithTimes);
            }

            return new State(currentActions, fluents, impossibleActions, futureActions);
        }
    }
}
