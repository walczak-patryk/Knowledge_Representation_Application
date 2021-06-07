using System;
using System.Collections.Generic;
using System.Linq;
using KR_Lib.DataStructures;
using KR_Lib.Structures;

namespace KR_Lib.Queries
{
    /// <summary>
    /// Zapytanie użytkownika czy podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek
    /// </summary>
    public class PossibleScenarioQuery : IQuery
    {
        public Guid ScenarioId
        {
            get;
        }

        public PossibleScenarioQuery(Guid scenarioId)
        {
            this.ScenarioId = scenarioId;
        }

        /// <summary>
        /// Odpowiedź na pytanie czy podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek
        /// </summary>
        /// <param name="modeledStructures">Lista modeli i niespójnych struktur, dla których sprawdzana będzie prawdziwość query</param>
        /// <returns>Prawda jeżeli podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek w stukturach z listy, fałsz w.p.p.<returns>
        public bool GetAnswer(List<IStructure> modeledStructures)
        {
            //var models = modeledStructures.Where(s => s is Model);
            if (modeledStructures.Any())
                return true;
            return false;
        }
    }
}
