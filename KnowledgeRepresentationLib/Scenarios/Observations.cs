using KR_Lib.Formulas;
using KR_Lib.Tree;

using System;

namespace KR_Lib.Scenarios
{
    public class Observations
    {
        public Guid Id { get; set; }
        string Name { get; set; }
        IFormula Form { get; set; }
        public int Time { get; set; }
        public Observations(string name, IFormula formula, int time) : base()
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Form = formula;
            this.Time = time;
        }

        /// <summary>
        /// We check if all fluents of parameter state are equal to matching fluents stored in this object.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CheckState(State state, int time)
        {
            if (!time.Equals(Time))
            {
                return true;
            }
            return Form.Evaluate();
        }
        public override string ToString()
        {
            return "Expression " + Form.ToString() + " in time " + Time.ToString();
        }
        //public void MetodaPoglądowa() 
        //{
        //    IFormula formula = new Formula(new Fluent("loaded", false));
        //    formula = new NegationFormula(formula);
        //    formula.Evaluate();
        //}
    }
}
