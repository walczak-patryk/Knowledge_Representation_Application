using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {
        public ReleaseStatement(Action action, Fluent fluent, Formula formula) : base(action, fluent, formula)
        {
            if (formula.Evaluate())
            {
                fluent.State = true;
            }
            // TODO: uzupełnić logiki w Nodzie - jeżeli Akcja już zakończyła działanie to dopiero tworzony ten Statement
        }
    }
}
