using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ReleaseStatement2 : Statement
    {
        public ReleaseStatement2(Action action, Fluent fluent, Formula formula = null) : base(action, fluent, formula) { }
    }
}
