using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {
        private Formula formulaCaused;

        public ReleaseStatement(Action action, Fluent fluent, Formula formula = null) : base(action, null, formula) { this.formulaCaused = formulaCaused; }

    }
}
