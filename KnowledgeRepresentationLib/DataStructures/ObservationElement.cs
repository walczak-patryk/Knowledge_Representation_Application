using KR_Lib.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Lib.DataStructures
{
    public class ObservationElement
    {
        public bool isFluent { get; set; }
        public Fluent fluent { get; set; }
        public int length { get; set; }
        public string operator_ { get; set; }
        public IFormula formula { get;set;}

        public ObservationElement(bool isFluent, Fluent fluent, int length, string operator_)
        {
            this.isFluent = isFluent;
            this.fluent = fluent;
            this.length = length;
            this.operator_ = operator_;
        }
    }
}
