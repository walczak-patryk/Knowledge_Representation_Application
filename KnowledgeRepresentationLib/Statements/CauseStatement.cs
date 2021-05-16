using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private Formula formulaCaused;

        public CauseStatement(Action action, Fluent fluent, Formula formula, Formula formulaCaused) : base(action, null, formula)
        {
            if (formula.Evaluate())
            {
                this.formulaCaused = formulaCaused;
            }
        }


    }
}
