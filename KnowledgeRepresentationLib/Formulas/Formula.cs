using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas {
    public interface IFormula
    {
        bool Evaluate();

        List<Fluent> GetFluents();
    
    }

    public class Formula : IFormula
    {
        Fluent fluent;

        public Formula(Fluent fluent)
        {
            this.fluent = fluent;
        }
        public bool Evaluate()
        {
            return this.fluent.State;
        }

        public List<Fluent> GetFluents()
        {
            return new List<Fluent>() { this.fluent };
        }
    }
}
