using KR_Lib.Scenarios;
using KR_Lib.Structures;
using System;
using System.Collections.Generic;

namespace KR_Lib.Queries
{
    /// <summary>
    /// Zapytanie użytkownika czy w chwili t realizacji scenariusza wykonywana jest akcja A
    /// </summary>
    public class ActionQuery : IQuery
    {
        int time;
        Action action;

        public Action Action
        {
            get
            {
                return action;
            }
        }
        public int Time
        {
            get
            {
                return time;
            }
        }

        /// <summary>
        /// Odpowiedź na pytanie czy w chwili t realizacji scenariusza wykonywana jest akcja A
        /// </summary>
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>Prawda jeżeli akcja A jest wykonywana w chwili t w każdej stukturze z listy, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures, IScenario scenario)
        {
            bool atLeastOneModel = false;
            foreach (var structure in modeledStructures)
            {
                if (structure is Model)
                {
                    atLeastOneModel = true;
                    if (structure.CheckActionBelongingToE(this.Action, this.Time) != 1)
                        return false;
                }
            }
            return atLeastOneModel && true;
        }
    }
}
