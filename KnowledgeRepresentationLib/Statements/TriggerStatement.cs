using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class TriggerStatement : Statement
    {
        IFormula formulaIf;
        private bool doFlag = false;

        public TriggerStatement(Action action, IFormula formulaIf) : base(action)
        {
            this.formulaIf = formulaIf;
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
            formulaIf.SetFluentsStates(fluents);
            doFlag = formulaIf.Evaluate();
            return doFlag;
        }

        public override List<State> DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions, int time)
        {
            if (doFlag)
            {
                currentActions.Add(action as ActionWithTimes);
            }
            
            return new List<State>(){new State(currentActions, fluents, impossibleActions, futureActions)};
        }

        public override bool GetDoFlag()
        {
            return doFlag;
        }
    }
}

