using Action = KR_Lib.DataStructures.Action;
using KR_Lib.Structures;
using System;
using System.Collections.Generic;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Queries
{
    /// <summary>
    /// Zapytanie użytkownika czy w chwili t realizacji scenariusza wykonywana jest akcja A
    /// </summary>
    public class ActionQuery : IQuery
    {
        int time;
        Action action;

        public Guid ScenarioId 
        { 
            get; 
        }

        public ActionQuery(int time, Action action, Guid scenarioId)
        {
            this.ScenarioId = scenarioId;
            this.time = time;
            this.action = action;
        }

        /// <summary>
        /// Odpowiedź na pytanie czy w chwili t realizacji scenariusza wykonywana jest akcja A
        /// </summary>
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <returns>Prawda jeżeli akcja A jest wykonywana w chwili t w każdej stukturze z listy, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures)
        {
            bool atLeastOneModel = false;
            foreach (var structure in modeledStructures)
            {
                if (structure is Model)
                {
                    atLeastOneModel = true;
                    if (structure.CheckActionBelongingToE(this.action, this.time) != true)
                        return false;
                }
            }
            return atLeastOneModel && true;
        }
    }
}
