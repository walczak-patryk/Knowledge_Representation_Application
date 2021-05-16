using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System;
using System.Collections.Generic;

namespace KR_Lib.Scenarios
{
    public interface IScenario
    {

        public List<DataStructures.Observation> GetObservations()
        {
            return observations;
        }
        public int GetScenarioDuration()
        {
            int durationObs = 0;
            int durationAcs = 0;
            foreach (DataStructures.Action acs in actions)
            {
                durationAcs += acs.DurationTime;
            }
            foreach (DataStructures.Observation obs in observations)
            {
                durationObs = Math.Max(durationObs, obs.Time);
            }
            return Math.Max(durationAcs, durationObs);
        }

        public Scenario()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
