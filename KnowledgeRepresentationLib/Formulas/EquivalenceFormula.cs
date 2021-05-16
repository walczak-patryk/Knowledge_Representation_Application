namespace KR_Lib.Formulas
{
    public class EquivalenceFormula : IFormula
    {
        IFormula formula;
        IFormula formula2;

        public EquivalenceFormula(IFormula formula, IFormula formula2)
        {
            this.formula = formula;
            this.formula2 = formula2;
        }
        public bool Evaluate()
        {
            return this.formula.Evaluate() == this.formula2.Evaluate();
        }
    }
}
