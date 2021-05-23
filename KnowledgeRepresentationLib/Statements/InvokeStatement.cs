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
        private int? waitTime;
        bool ifFlag = false;
        bool waitTimeFlag = false;

        public InvokeStatement(ActionTime action, ActionTime actionInvoked, IFormula formulaIf = null, int? waitTime = null) : base(action)
        {
            this.actionInvoked = actionInvoked;
            if (formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }

            if (waitTime != null)
            {
                waitTimeFlag = true;
                this.waitTime = waitTime;
            }
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            // sprawdzić current action == action
            if (!(currentAction is ActionWithTimes))
            {
                throw new Exception("Niewłaściwy typ w InvokeStatement: potrzebny ActionWithTimes");
            }

            var actionWithTimes = (currentAction as ActionWithTimes);
            bool result = true;
            int? startTime = actionWithTimes.GetEndTime() + waitTime;
            if (ifFlag)
            {
                if (waitTimeFlag)
                {
                    result = formulaIf.Evaluate() && currentTime == startTime;
                } else
                {
                    result =  formulaIf.Evaluate();
                }
            }

            if (result)
            {
                this.actionInvoked = new ActionWithTimes(actionInvoked, (actionInvoked as ActionTime).Time, startTime.Value);
            }
            return result;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions)
        {
            currentActions.Add(actionInvoked as ActionWithTimes);
            return new State(currentActions, fluents, impossibleActions);
        }
    }
}
