using System;
using System.Collections.Generic;
using System.Linq;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas
{
    public class ConjunctionFormula : IFormula
    {
        List<IFormula> formulas = new List<IFormula>();

        public ConjunctionFormula(IFormula formula1, IFormula formula2)
        {
            this.formulas.Add(formula1);
            this.formulas.Add(formula2);
        }

        public ConjunctionFormula(IFormula formula1, IFormula formula2, IFormula formula3) : this(formula1, formula2)
        {
            this.formulas.Add(formula3);
        }

        public ConjunctionFormula(IFormula formula1, IFormula formula2, IFormula formula3, IFormula formula4) : this(formula1, formula2, formula3)
        {
            this.formulas.Add(formula4);
        }

        public override bool Evaluate()
        {
            if (this.formulas.Count == 0)
                new Exception("Invalid Conjuction Formula");

            foreach (var formula in formulas)
                if (!formula.Evaluate())
                    return false;
            return true;
        }
        public override HashSet<Fluent> GetFluents()
        {

            HashSet<Fluent> fluents = new HashSet<Fluent>();
            foreach (var formula in formulas)
                fluents.Union(formula.GetFluents());

            return fluents;
        }

        public override List<HashSet<Fluent>> GetStatesFluents(bool state)
        {
            List<HashSet<Fluent>> listOfFluents = new List<HashSet<Fluent>>();
            if (state)
            {   
                listOfFluents = this.formulas[0].GetStatesFluents(true);
                for(int i = 1; i<formulas.Count; i++){
                    var statesFluents = this.formulas[i].GetStatesFluents(true);
                    var tmpList = new List<HashSet<Fluent>>();
                    foreach(var f in listOfFluents){
                        for(int j = 0; j<statesFluents.Count; j++){
                            var newSet = f.Select(flu => (flu.Clone() as Fluent)).ToHashSet();
                            if(this.CheckSetsAreValid(newSet, statesFluents[j])){
                                newSet.Union(statesFluents[j]);
                                tmpList.Add(newSet);
                            }
                        }
                    }
                    listOfFluents = tmpList;
                }
                    
            }
            else
            {
                var combinations = TreeMethods.GenerateBoolCombinations(this.formulas.Count);
                foreach (var combination in combinations)
                {
                    if(combination.Contains(false)){
                        var iterationListOfFluents = this.formulas[0].GetStatesFluents(combination[0]);
                        for(int j = 1; j<formulas.Count; j++){
                            var statesFluents = this.formulas[j].GetStatesFluents(combination[j]);
                            var tmpList = new List<HashSet<Fluent>>();
                            foreach(var f in iterationListOfFluents){
                                for(int k = 0; k<statesFluents.Count; k++){
                                    var newSet = f.Select(flu => (flu.Clone() as Fluent)).ToHashSet();
                                    if(this.CheckSetsAreValid(newSet, statesFluents[k])){
                                        newSet.Union(statesFluents[k]);
                                        tmpList.Add(newSet);
                                    }
                                }
                            }
                            iterationListOfFluents = tmpList;
                        }
                        listOfFluents.AddRange(iterationListOfFluents);
                    }                       
                }
            }           

            return listOfFluents;
        }

        public override void SetFluentsStates(List<Fluent> fluents)
        {
            foreach (var formula in formulas)
                formula.SetFluentsStates(fluents);
        }
    }
}
