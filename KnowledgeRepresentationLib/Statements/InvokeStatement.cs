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
            int? currentDurationTime = currentTime - currentAction.StartTime;
            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                result = formulaIf.Evaluate() && currentDurationTime == (action as ActionTime).Time;
            } else
            {
                result = currentDurationTime == (action as ActionTime).Time;
            }

            if (result)
            {
                this.actionInvokedWithTimes = new ActionWithTimes(actionInvoked, (actionInvoked as ActionTime).Time, currentAction.GetEndTime() + waitTime);
            }
            return result;
        }

        public List<(State, HashSet<Fluent>)> DoStatement(State newState, int time)
        {
            if (actionInvokedWithTimes.StartTime == time)
                newState.CurrentActions.Add(actionInvokedWithTimes);
            else
                newState.FutureActions.Add(actionInvokedWithTimes);
            return new List<(State, HashSet<Fluent>)>() { (newState, null) };
        }

        public override List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                return this.DoStatement(newState, time);
            return null;
        }
    }
}
