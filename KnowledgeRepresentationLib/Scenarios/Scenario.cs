using KnowledgeRepresentationLib.Scenarios;
using System;

namespace KR_Lib.Scenarios
{
    public interface IScenario
    {

    }
    public class Scenario : IScenario
    {
        internal Observations GetObservations()
        {
            throw new NotImplementedException();
        }
    }
}
