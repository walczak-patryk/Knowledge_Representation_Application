using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public interface IFormula
    {
        bool Evaluate();

        List<Fluent> GetFluents();

        void SetFluentsStates(List<Fluent> fluents);

        List<List<Fluent>> GetStatesFluents(bool state);

    }

    public class Formula : IFormula
    {
        public Fluent fluent { get; }

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

        public  void SetFluentsStates(List<Fluent> fluents)
        {
            foreach(var f in fluents)
            {
                if (this.fluent == f)
                    this.fluent.State = f.State;
            }
        }

        public List<List<Fluent>> GetStatesFluents(bool state)
        {
            var copyFluent = this.fluent.Clone() as Fluent;
            copyFluent.State = state;
            return new List<List<Fluent>>() { new List<Fluent>() { copyFluent } };
        }
    }
}
