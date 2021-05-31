using System.Collections.Generic;
using KR_Lib.DataStructures;

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
        public List<Fluent> GetFluents()
        {
            List<Fluent> fluents = this.formula.GetFluents();
            List<Fluent> fluents2 = this.formula2.GetFluents();
            foreach (Fluent fluent in fluents2)
            {
                fluents.Add(fluent);
            }

            return fluents;
        }

        public void SetFluentsStates(List<Fluent> fluents)
        {
            this.formula.SetFluentsStates(fluents);
            this.formula2.SetFluentsStates(fluents);
        }
        
        public List<List<Fluent>> GetStatesFluents(bool state)
        {
            List<List<Fluent>> listOfFluents = new List<List<Fluent>>();
            if (state)
            {
                var result1 = this.formula.GetStatesFluents(true);
                var result2 = this.formula2.GetStatesFluents(true);
                foreach(var res1 in result1){
                    foreach(var res2 in result2){
                        res1.AddRange(res2);
                    }
                }

                var result3 = this.formula.GetStatesFluents(false);
                var result4 = this.formula2.GetStatesFluents(false);
                foreach(var res3 in result3){
                    foreach(var res4 in result4){
                        res3.AddRange(res4);
                    }
                }
                listOfFluents.AddRange(result1);
                listOfFluents.AddRange(result3);
            }
            else
            {
                var result1 = this.formula.GetStatesFluents(true);
                var result2 = this.formula2.GetStatesFluents(false);
                foreach(var res1 in result1){
                    foreach(var res2 in result2){
                        res1.AddRange(res2);
                    }
                }

                var result3 = this.formula.GetStatesFluents(false);
                var result4 = this.formula2.GetStatesFluents(true);
                foreach(var res3 in result3){
                    foreach(var res4 in result4){
                        res3.AddRange(res4);
                    }
                }
                listOfFluents.AddRange(result1);
                listOfFluents.AddRange(result3);
            }           

            return listOfFluents;
        }
    }
}
