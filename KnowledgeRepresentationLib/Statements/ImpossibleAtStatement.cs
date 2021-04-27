using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KR_Lib.Statements
{
    public class ImpossibleAtStatement : Statement
    {
        private int time;
        public ImpossibleAtStatement(Action action, int time) : base(action, null, null) { this.time = time; }
    }
}
