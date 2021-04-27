using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {
        public ReleaseStatement(Action action, Fluent fluent, Formula formula = null) : base(action, fluent, formula) { }

    }
}
