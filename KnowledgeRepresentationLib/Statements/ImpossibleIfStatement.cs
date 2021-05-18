using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ImpossibleIfStatement : Statement
    {
        public ImpossibleIfStatement(Action action, Formula formula = null) : base(action, null, formula) { }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, int time)
        {
            return formula.Evaluate();
        }

        public override State DoStatement(Action currentAction, List<Fluent> fluents)
        {
            throw new System.NotImplementedException();
        }
    }
}
