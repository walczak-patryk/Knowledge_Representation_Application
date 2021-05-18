using KR_Lib.DataStructures;
using System.Collections.Generic;

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
            return new List<Fluent>() { fluent };
        }
    }
}
