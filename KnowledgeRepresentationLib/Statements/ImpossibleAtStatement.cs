using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ImpossibleAtStatement : Statement
    {
        private int time;

        public ImpossibleAtStatement(Action action, int time) : base(action, null, null)
        {
            this.time = time;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, int currentTime)
        {
            if (time == currentTime)
            {
                return true;
            }
            return false;
        }
    }
}
