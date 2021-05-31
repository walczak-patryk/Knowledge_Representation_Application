using System;
using System.Collections.Generic;
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

        public bool Evaluate()
        {
            if (this.formulas.Count == 0)
                new Exception("Invalid Conjuction Formula");

            foreach (var formula in formulas)
                if (!formula.Evaluate())
                    return false;
            return true;
        }
        public List<Fluent> GetFluents()
        {

            List<Fluent> fluents = new List<Fluent>();
            foreach (var formula in formulas)
                fluents.AddRange(formula.GetFluents());

            return fluents;
        }

        public List<List<Fluent>> GetStatesFluents(bool state)
        {
            List<List<Fluent>> listOfFluents = new List<List<Fluent>>();
            if (state)
            {
                foreach (var formula in formulas)
                    listOfFluents.AddRange(formula.GetStatesFluents(state));
            }
            else
            {
                var combinations = TreeMethods.GenerateBoolCombinations(this.formulas.Count);
                foreach (var combination in combinations)
                {
                    if(combination.Contains(false)){
                        for(int i = 0; i< combination.Count; i++)
                            listOfFluents.AddRange(this.formulas[i].GetStatesFluents(combination[i]));
                    }                       
                }
            }           

            return listOfFluents;
        }

        public void SetFluentsStates(List<Fluent> fluents)
        {
            foreach (var formula in formulas)
                formula.SetFluentsStates(fluents);
        }
    }
}
