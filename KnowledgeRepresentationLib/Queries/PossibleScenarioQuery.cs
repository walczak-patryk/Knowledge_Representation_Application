using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;
using KR_Lib.Scenarios;
using KR_Lib.Structures;

namespace KR_Lib.Queries 
{
    /// <summary>
    /// Zapytanie użytkownika czy podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek
    /// </summary>
    public class PossibleScenarioQuery : IQuery
    {

        public QueryType QueryType 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Odpowiedź na pytanie czy podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek
        /// </summary>
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>Prawda jeżeli podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek w stukturach z listy, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures, IScenario scenario)
        {
            bool atLeatOneModel = false;
            bool atLeatOneInconsistent = false;
            foreach (var model in modeledStructures)
            {
                if (model is InconsistentStructure) atLeatOneInconsistent = true;
                else if (model is Model) atLeatOneModel = true;
            }
            if (this.QueryType == QueryType.Ever) return atLeatOneModel;
            else return !atLeatOneInconsistent;
        }
    }
}
