﻿using System.Collections.Generic;
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
        public override bool Evaluate()
        {
            return !this.formula.Evaluate();
        }
        public override HashSet<Fluent> GetFluents()
        {
            return this.formula.GetFluents();
        }

        public override List<HashSet<Fluent>> GetStatesFluents(bool state)
        {
            return formula.GetStatesFluents(!state);
        }

        public override void SetFluentsStates(List<Fluent> fluents)
        {
            this.formula.SetFluentsStates(fluents);
        }
    }
}
