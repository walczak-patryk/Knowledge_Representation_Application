using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public class AlternativeFormula : IFormula
    {
        List<IFormula> formulas = new List<IFormula>();

        public AlternativeFormula(IFormula formula1, IFormula formula2)
        {
            this.formulas.Add(formula1);
            this.formulas.Add(formula2);
        }
        public bool Evaluate()
        {
            if (this.formulas.Count == 0)
                new Exception("Invalid Alternative Formula");

            foreach (var formula in formulas)
                if (formula.Evaluate())
                    return true;
            return false;
        }
        public List<Fluent> GetFluents()
        {
            List<Fluent> fluents = new List<Fluent>();
            foreach (var formula in formulas)
                fluents.AddRange(formula.GetFluents());

            return fluents;
        }

        public void SetFluentsStates(List<Fluent> fluents)
        {
            foreach (var formula in formulas)
                formula.SetFluentsStates(fluents);
        }
    }
}
