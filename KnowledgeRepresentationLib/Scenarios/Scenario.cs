using KnowledgeRepresentationLib.Scenarios;
using System;

namespace KR_Lib.Scenarios
{
    public interface IScenario
    {
        Guid Id { get; }
    }
    public class Scenario : IScenario
    {
        public Guid Id
        {
            get;
        }


        public Observations GetObservations()
        {
            throw new NotImplementedException();
        }

        public Scenario()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
