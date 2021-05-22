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

        public InvokeStatement(ActionWithTimes action, ActionTime actionInvoked, IFormula formulaIf = null, int? waitTime = null) : base(action)
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

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int currentTime)
        {
            if (!(currentAction is ActionWithTimes))
            {
                throw new Exception("Niewłaściwy typ w InvokeStatement: potrzebny ActionWithTimes");
            }

            var actionWithTimes = (currentAction as ActionWithTimes);

            if(ifFlag)
            {
                if (waitTimeFlag)
                {
                    return formulaIf.Evaluate() == true && currentTime == actionWithTimes.GetEndTime() + waitTime && !impossibleActions.Contains(actionInvoked);
                } else
                {
                    return formulaIf.Evaluate() == true && !impossibleActions.Contains(actionInvoked);
                }
            }

            return !impossibleActions.Contains(actionInvoked);
        }

        public override State DoStatement(List<Action> currentActions, List<Fluent> fluents, List<Action> impossibleActions)
        {
            currentActions.Add(actionInvoked);
            return new State(currentActions, fluents, impossibleActions);
        }
    }
}
