using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ImpossibleIfStatement : Statement
    {
        IFormula formulaIf;

        public ImpossibleIfStatement(Action action, IFormula formulaIf = null) : base(action)
        {
            this.formulaIf = formulaIf;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int time)
        {
            return formulaIf.Evaluate();
        }

        public override State DoStatement(List<Action> currentActions, List<Fluent> fluents, List<Action> impossibleActions)
        {
            impossibleActions.Add(action);
            return new State(currentActions, fluents, impossibleActions);
        }
    }
}
