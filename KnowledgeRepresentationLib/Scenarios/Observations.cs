using KR_Lib.DataStructures;
using KR_Lib.Formulas;

namespace KnowledgeRepresentationLib.Scenarios
{
    public class Observations
    {
        public void MetodaPoglądowa() 
        {
            IFormula formula = new Formula(new Fluent("loaded", false));
            formula = new NegationFormula(formula);
            formula.Evaluate();
        }
    }
}
