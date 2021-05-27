using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {

        private Fluent fluent;
        private IFormula formulaIf;
        private bool doFlag = false;
        bool ifFlag = false;

        public ReleaseStatement(Action action, Fluent fluent, IFormula formulaIf) : base(action)
        {
            this.fluent = fluent;
            if (formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            if (action != currentAction)
            {
                doFlag = false;
                return doFlag;
            }

            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                doFlag = currentTime - currentAction.StartTime == (action as ActionTime).Time && formulaIf.Evaluate();
                return doFlag;
            }
            
            doFlag = currentTime - currentAction.StartTime == (action as ActionTime).Time;
            return doFlag;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions)
        {
            if (doFlag)
            {
                fluents.Find(f => f == fluent).State = !fluents.Find(f => f == fluent).State;
            }
            
            return new State(currentActions, fluents, impossibleActions, futureActions);
        }

    }
}
