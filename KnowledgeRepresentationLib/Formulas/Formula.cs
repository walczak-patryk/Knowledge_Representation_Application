using KR_Lib.DataStructures;
using KR_Lib.Tree;

namespace KR_Lib.Formulas {
    public interface IFormula
    {
        bool Evaluate(State state);
    
    }

    public class Formula : IFormula
    {
        Fluent fluent;

        public Formula(Fluent fluent)
        {
            this.fluent = fluent;
        }
        public bool Evaluate(State state)
        {
            return this.fluent.State;
        }
    }
}
