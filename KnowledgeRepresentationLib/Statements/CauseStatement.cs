using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private IFormula formulaCaused;
        private IFormula formulaIf;
        private bool doFlag = false;
        bool ifFlag = false;

        public CauseStatement(ActionTime action, IFormula formulaCaused, IFormula formulaIf = null) : base(action)
        {
            this.formulaCaused = formulaCaused;
            if(formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
            if (action != currentAction)
            {
                doFlag = false;
                return false;
            }
            // if działa aktualnie tylko z formula o wartości true
            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                doFlag = (currentAction == action && time - currentAction.StartTime == (action as ActionTime).Time && formulaIf.Evaluate());
            }
            else
                doFlag = (currentAction == action &&  time - currentAction.StartTime == (action as ActionTime).Time);

            return doFlag;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions)
        {
            if (doFlag)
            {
                foreach (Fluent fluent in formulaCaused.GetFluents())
                {
                    fluents.Find(f => f == fluent).State = !fluents.Find(f => f == fluent).State;
                }
            }

            return new State(currentActions, fluents, impossibleActions, futureActions);
        }

    }
}
