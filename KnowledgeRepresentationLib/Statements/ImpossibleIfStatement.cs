using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ImpossibleIfStatement : Statement
    {
        IFormula formulaIf;
        private bool doFlag;

        public ImpossibleIfStatement(Action action, IFormula formulaIf = null) : base(action)
        {
            this.formulaIf = formulaIf;
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
            formulaIf.SetFluentsStates(fluents);
            doFlag = formulaIf.Evaluate();
            return doFlag;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions)
        {
            if (doFlag)
            {
                var actionWTime = (action as ActionWithTimes);
                impossibleActions.Add(actionWTime);
            }

            return new State(currentActions, fluents, impossibleActions, futureActions);
        }
    }
}
