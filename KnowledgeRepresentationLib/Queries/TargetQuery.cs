using System;
using System.Collections.Generic;
using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Scenarios;
using KR_Lib.Structures;

namespace KR_Lib.Queries 
{
    /// <summary>
    /// Zapytanie użytkownika czy podany cel γ jest osiągalny zawsze/kiedykolwiek przy zadanym zbiorze obserwacji OBS?
    /// </summary>
    public class TargetQuery : IQuery
    {

        Formula formula;

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
        /// Odpowiedź na pytanie o spełnialność formuły przy ustalonym scenariuszu - w ciągu 
        /// całego czasu trwania (od 0 do końca ostatniej akcji)
        /// </summary>
        /// <param name="models">Lista modeli i niespójnych struktur</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>bool</returns>
        public bool GetAnswer(List<IStructure> models, IScenario scenario)
        {
            List<int> possibleTimes = new List<int>();
            bool atLeatOneTrue = false;
            bool atLeatOneFalse = false;
            foreach (var model in models)
            {
                possibleTimes.Clear(); 
                int time = model.EndTime;
                for (int i=0; i<time; i++)
                {
                    bool evaluationResult = model.EvaluateFormula(this.formula, i);
                    if (evaluationResult) possibleTimes.Add(i);
                }
                if (possibleTimes.Count > 0) atLeatOneTrue = true;
                else atLeatOneFalse = true;
            }
            if (this.QueryType == QueryType.Ever) return atLeatOneTrue;
            else return !atLeatOneFalse;
        }
    }
}
