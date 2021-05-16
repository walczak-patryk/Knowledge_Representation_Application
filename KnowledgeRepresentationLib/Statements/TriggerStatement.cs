using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class TriggerStatement : Statement
    {
        public TriggerStatement(Action action, Fluent fluent, Formula formula) : base(action, null, formula)
        {
            // logika chyba w Nodzie - stworzenie tej akcji? TODO
        }
    }
}
