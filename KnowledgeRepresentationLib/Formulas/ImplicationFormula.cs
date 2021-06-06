using System.Collections.Generic;
using System.Linq;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public class ImplicationFormula : IFormula
    {
        IFormula formula;
        IFormula formula2;

        public ImplicationFormula(IFormula formula, IFormula formula2)
        {
            this.formula = formula;
            this.formula2 = formula2;
        }
        public override bool Evaluate()
        {
            return !this.formula.Evaluate() || this.formula2.Evaluate();
        }
        public override HashSet<Fluent> GetFluents()
        {
            HashSet<Fluent> fluents = this.formula.GetFluents();
            HashSet<Fluent> fluents2 = this.formula2.GetFluents();
            fluents.Union(fluents2);

            return fluents;
        }
        public override void SetFluentsStates(List<Fluent> fluents)
        {
            this.formula.SetFluentsStates(fluents);
            this.formula2.SetFluentsStates(fluents);
        }

         public override List<HashSet<Fluent>> GetStatesFluents(bool state)
        {
            List<HashSet<Fluent>> listOfFluents = new List<HashSet<Fluent>>();
            if (state)
            {
                var result1 = this.formula.GetStatesFluents(true);
                var result2 = this.formula2.GetStatesFluents(true);
                foreach(var res1 in result1){
                    foreach(var res2 in result2){
                        if(CheckSetsAreValid(res1,res2))
                            res1.Union(res2);
                    }
                }

                var result3 = this.formula.GetStatesFluents(false);
                var result4 = this.formula2.GetStatesFluents(false);
                foreach(var res3 in result3){
                    foreach(var res4 in result4){
                        if(CheckSetsAreValid(res3,res4))
                            res3.Union(res4);
                    }
                }

                var result5 = this.formula.GetStatesFluents(false);
                var result6 = this.formula2.GetStatesFluents(true);
                foreach(var res5 in result5){
                    foreach(var res6 in result6){
                        if(CheckSetsAreValid(res5,res6))
                            res5.Union(res6);
                    }
                }
                listOfFluents.AddRange(result1);
                listOfFluents.AddRange(result3);
                listOfFluents.AddRange(result5);
            }
            else
            {
                var result1 = this.formula.GetStatesFluents(false);
                var result2 = this.formula2.GetStatesFluents(true);
                foreach(var res1 in result1){
                    foreach(var res2 in result2){
                        if(CheckSetsAreValid(res1,res2))
                            res1.Union(res2);
                    }
                }
                listOfFluents.AddRange(result1);
            }           

            return listOfFluents;
        }
    }
}
