using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        private Action actionInvoked;
        private ActionWithTimes actionInvokedWithTimes;
        private IFormula formulaIf;
        private int waitTime;
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

        public bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            if (action != currentAction)
            {
                return false;
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
                this.actionInvokedWithTimes = new ActionWithTimes(actionInvoked, (actionInvoked as ActionTime).Time, startTime.Value);
            }
            return result;
        }

        public void DoStatement(ref List<State> newStates, int time)
        {
            foreach (var state in newStates)
            {
                if (actionInvokedWithTimes.StartTime == time)
                    state.CurrentActions.Add(actionInvokedWithTimes);
                else
                    state.FutureActions.Add(actionInvokedWithTimes);
            }
        }

        public override void CheckAndDo(State parentState, ref List<State> newStates, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                this.DoStatement(ref newStates, time);
            return;
        }
    }
}
