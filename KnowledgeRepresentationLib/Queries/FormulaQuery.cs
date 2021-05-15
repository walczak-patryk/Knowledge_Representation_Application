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
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>Prawda jeżeli podana formuła jest prawdziwa przy każdej/przynajmniej jednej strukturze z listy, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures, IScenario scenario)
        {
            bool atLeatOneTrue = false;
            bool atLeastOneFalse = false;
            bool atLeastOneModel = false;
            foreach (var structure in modeledStructures)
            {
                if (structure is Model)
                {
                    atLeastOneModel = true;
                    bool evaluationResult = structure.EvaluateFormula(this.formula, this.Time);
                    if (evaluationResult) atLeatOneTrue = true;
                    else atLeastOneFalse = true;
                }
            }
            if (this.QueryType == QueryType.Ever) return atLeastOneModel && atLeatOneTrue;
            else return atLeastOneModel && !atLeastOneFalse;
        }
    }
    
}
