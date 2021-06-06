using System.Collections.Generic;
using System.Linq;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public abstract class IFormula
    {
        public abstract bool Evaluate();

        public abstract HashSet<Fluent> GetFluents();

        public abstract void SetFluentsStates(List<Fluent> fluents);

        public abstract List<HashSet<Fluent>> GetStatesFluents(bool state);

        protected bool CheckSetsAreValid(HashSet<Fluent> set1, HashSet<Fluent> set2){
            foreach(var fluent in set1){
                var v = set2.Where(s => s.Id == fluent.Id).SingleOrDefault();
                if(v != null && v.State != fluent.State){
                    return false;
                }
            }
            return true;
        }

    }

    public class Formula : IFormula
    {
        public Fluent fluent { get; }

        public Formula(Fluent fluent)
        {
            this.fluent = fluent;
        }
        public override bool Evaluate()
        {
            return this.fluent.State;
        }

        public override HashSet<Fluent> GetFluents()
        {
            return new HashSet<Fluent>() { this.fluent };
        }

        public override void SetFluentsStates(List<Fluent> fluents)
        {
            foreach(var f in fluents)
            {
                if (this.fluent == f)
                    this.fluent.State = f.State;
            }
        }

        public override List<HashSet<Fluent>> GetStatesFluents(bool state)
        {
            var copyFluent = this.fluent.Clone() as Fluent;
            copyFluent.State = state;
            return new List<HashSet<Fluent>>() { new HashSet<Fluent>() { copyFluent } };
        }
    }
}
