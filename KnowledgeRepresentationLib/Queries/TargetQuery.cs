using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Structures;

namespace KR_Lib.Queries
{
    /// <summary>
    /// Zapytanie użytkownika czy podany cel γ jest osiągalny zawsze/kiedykolwiek przy zadanym zbiorze obserwacji OBS?
    /// </summary>
    public class TargetQuery : IQuery
    {

        private Formula formula;
        private QueryType queryType;

        public Guid ScenarioId
        {
            get;
        }

        public TargetQuery(Formula formula, QueryType queryType, Guid scenarioId)
        {
            this.ScenarioId = scenarioId;
            this.formula = formula;
            this.queryType = queryType;
        }

        /// <summary>
        /// Odpowiedź na pytanie o spełnialność formuły przy ustalonym scenariuszu - w ciągu 
        /// całego czasu trwania (od 0 do końca ostatniej akcji) dla przynajmniej jednego/każdego modelu
        /// </summary>
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <returns>Prawda jeżeli formuła jest prawdziwa dla każdej/przynajmniej jednej struktury w jakichkolwiek czasach z przedziału, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures)
        {
            List<int> possibleTimes = new List<int>();
            bool atLeatOneTrue = false;
            bool atLeatOneFalse = false;
            bool atLeastOneModel = false;
            foreach (var structure in modeledStructures)
            {
                if (structure is Model)
                {
                    atLeastOneModel = true;
                    possibleTimes.Clear();
                    int time = structure.EndTime;
                    for (int i = 0; i < time; i++)
                    {
                        bool evaluationResult = structure.EvaluateFormula(this.formula, i);
                        if (evaluationResult) possibleTimes.Add(i);
                    }
                    if (possibleTimes.Count > 0) atLeatOneTrue = true;
                    else atLeatOneFalse = true;
                }
            }
            if (this.queryType == QueryType.Ever) return atLeastOneModel &&  atLeatOneTrue;
            else return atLeastOneModel && !atLeatOneFalse;
        }
    }
}
