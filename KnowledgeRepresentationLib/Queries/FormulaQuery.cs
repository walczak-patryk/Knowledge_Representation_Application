using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Scenarios;
using KR_Lib.Structures;

namespace KR_Lib.Queries 
{
    /// <summary>
    /// Zapytanie użytkownika czy w chwili t ≥ 0 realizacji podanego scenariusza warunek γ zachodzi zawsze/kiedykolwiek?
    /// </summary>
    public class FormulaQuery : IQuery
    {
        
        private int time;
        private Formula formula;
        
        public int Time
        {
            get
            {
                return time;
            }
        }

        public Formula Formula
        {
            get
            {
                return formula;
            }
        }

        public QueryType QueryType
        {
            get;
            set;
        }

        /// <summary>
        /// Odpowiedź na pytanie czy w chwili t ≥ 0 realizacji podanego scenariusza 
        /// formuła jest prawdziwa zawsze/kiedykolwiek?
        /// </summary>
        /// <param name="models">Lista modeli i niespójnych struktur</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>bool</returns>
        public bool GetAnswer(List<IStructure> models, IScenario scenario)
        {
            bool atLeatOneTrue = false;
            bool atLeatOneFalse = false;
            foreach (var model in models)
            {
                bool evaluationResult = model.EvaluateFormula(this.formula, this.Time);
                if (evaluationResult) atLeatOneTrue = true;
                else atLeatOneFalse = true;
            }
            if (this.QueryType == QueryType.Ever) return atLeatOneTrue;
            else return !atLeatOneFalse;
        }
    }
    
}
