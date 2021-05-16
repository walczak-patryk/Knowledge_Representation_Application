using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ImpossibleAtStatement : Statement
    {
        //private int time; - czas w nodzie

        public ImpossibleAtStatement(Action action) : base(action, null, null)
        {
            //this.time = time;
        }
    }
}
