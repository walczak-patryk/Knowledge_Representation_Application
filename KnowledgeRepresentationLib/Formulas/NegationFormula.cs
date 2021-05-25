using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public class NegationFormula : IFormula
    {
        IFormula formula;

        public NegationFormula(IFormula formula)
        {
            this.formula = formula;
        }
        public bool Evaluate()
        {
            return !this.formula.Evaluate();
        }
        public List<Fluent> GetFluents()
        {
            return this.formula.GetFluents();
        }
        public void SetFluentsStates(List<Fluent> fluents)
        {
            this.formula.SetFluentsStates(fluents);
        }
    }
}
