using System;
using KR_Lib.Formulas;

namespace KR_Lib.Queries
{
    public class FormulaQuery : IQuery
    {
        Formula formula;
        int time;

        public int Time
        {
            get
            {
                return time;
            }
        }

        public Formula Formula
        {
            get
            {
                return formula;
            }
        }
    }
}
