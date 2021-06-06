using System;
using System.Collections.Generic;
using System.Linq;
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
        private IFormula formula;
        public QueryType queryType;

        public Guid ScenarioId
        {
            get;
        }

        public FormulaQuery(int time, IFormula formula, Guid scenarioId, QueryType queryType)
        {
            this.ScenarioId = scenarioId;
            this.time = time;
            this.formula = formula;
            this.queryType = queryType;
        }

        /// <summary>
        /// Odpowiedź na pytanie czy w chwili t ≥ 0 realizacji podanego scenariusza 
        /// formuła jest prawdziwa zawsze/kiedykolwiek?
        /// </summary>
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <returns>Prawda jeżeli podana formuła jest prawdziwa przy każdej/przynajmniej jednej strukturze z listy, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures)
        {
            bool atLeatOneTrue = false;
            bool atLeastOneFalse = false;
            var models = modeledStructures.Where(s => s is Model);
            foreach (var model in models)
            {
                bool evaluationResult = model.EvaluateFormula(this.formula, this.time);
                if (evaluationResult) atLeatOneTrue = true;
                else atLeastOneFalse = true;

            }
            if (this.queryType == QueryType.Ever) return atLeatOneTrue;
            else return !atLeastOneFalse;
        }
    }
    
}
