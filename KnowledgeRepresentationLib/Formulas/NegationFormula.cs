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
    }
}
