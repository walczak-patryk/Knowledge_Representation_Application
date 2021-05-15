using KR_Lib.Scenarios;
using KR_Lib.Structures;
using System.Collections.Generic;

namespace KR_Lib.Queries
{
    interface IQuery
    {
        /// <summary>
        /// Odpowiedź na pytanie użytkownika, działanie zależy od typu obiektu query
        /// </summary>
        /// <param name="models"></param>
        /// <param name="scenario"></param>
        /// <returns>bool</returns>
        bool GetAnswer(List<IStructure> models, IScenario scenario);
    }

}
