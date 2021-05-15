using KR_Lib.Scenarios;
using KR_Lib.Structures;
using System.Collections.Generic;

namespace KR_Lib.Queries
{
    interface IQuery
    {
        bool GetAnswer(List<IStructure> models, IScenario scenario);
    }

}
