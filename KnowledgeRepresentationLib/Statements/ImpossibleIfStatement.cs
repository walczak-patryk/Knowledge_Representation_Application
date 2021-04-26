using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ImpossibleIfStatement : Statement
    {
        public ImpossibleIfStatement(Action action, Formula formula = null) : base(action, null, formula) { }
    }
}
