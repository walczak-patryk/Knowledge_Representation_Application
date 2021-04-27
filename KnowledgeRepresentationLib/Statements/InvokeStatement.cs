using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class InvokeStatement : Statement
    {
        public InvokeStatement(Action action, Fluent fluent, Formula formula = null) : base(action, fluent, formula) { }

    }
}
