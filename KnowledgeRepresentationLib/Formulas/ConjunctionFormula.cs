using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public class ConjunctionFormula : IFormula
    {
        IFormula formula;
        IFormula formula2;

        public ConjunctionFormula(IFormula formula, IFormula formula2)
        {
            this.formula = formula;
            this.formula2 = formula2;
        }
        public bool Evaluate()
        {
            return this.formula.Evaluate() && this.formula2.Evaluate();
        }
        public List<Fluent> GetFluents()
        {
            List<Fluent> fluents = this.formula.GetFluents();
            List<Fluent> fluents2 = this.formula2.GetFluents();
            foreach (Fluent fluent in fluents2)
            {
                fluents.Add(fluent);
            }

            return fluents;
        }
    }
}
