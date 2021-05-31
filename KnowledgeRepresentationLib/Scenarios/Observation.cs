using System;
using KR_Lib.Formulas;

namespace KR_Lib.DataStructures
{
    public class Observation
    {
        public Guid Id
        {
            get;
            set;
        }

        public IFormula Formula
        {
            get;
            set;
        }
        public int Time
        {
            get;
            set;
        }

        public Observation() { }
        public Observation(IFormula formula, int time)
        {
            this.Formula = formula;
            this.Time = time;
            this.Id = Guid.NewGuid();
        }
        public override bool Equals(object obj)
        {
            if (obj is Observation)
            {
                var action = obj as Observation;
                if (action.Id.Equals(this.Id) && action.Time.Equals(this.Time))
                    return true;
            }
            return false;
        }
        public object Clone()
        {
            Observation Observation = new Observation();
            Observation.Id = Id;
            Observation.Formula = Formula;
            Observation.Time = Time;

            return Observation;
        }
    }
}
