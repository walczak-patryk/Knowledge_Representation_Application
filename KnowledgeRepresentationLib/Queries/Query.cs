using KR_Lib.Structures;
using System;
using System.Collections.Generic;

namespace KR_Lib.Queries
{
    interface IQuery
    {
        /// <summary>
        /// Odpowiedź na pytanie użytkownika, działanie zależy od typu obiektu query
        /// </summary>
        /// <param name="modeledStructures"></param>
        /// <returns>bool</returns>
        bool GetAnswer(List<IStructure> modeledStructures);

        Guid ScenarioId { get; }
    }

}
