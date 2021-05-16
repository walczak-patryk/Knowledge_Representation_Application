using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System;
using System.Collections.Generic;

namespace KR_Lib.Scenarios
{
    public interface IScenario
    {
        string Name { get; set; }
        Guid Id { get; set; }
        //List<DataStructures.Observation> observations { get; set; }
        //List<DataStructures.Action> actions { get; set; }
    }
    public class Scenario : IScenario
    {
        public string Name { get; set; }
        public Guid Id { get ; set ; }
        public List<DataStructures.Observation> observations { get; set; }
        public List<DataStructures.Action> actions { get; set; }
        public Scenario(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid();
            this.observations = new List<DataStructures.Observation>();
            this.actions = new List<DataStructures.Action>();
        }
        public void addObservation(string name, IFormula formula, int time)
        {
            DataStructures.Observation OBS = new DataStructures.Observation(name, formula, time);
            observations.Add(OBS);
        }
        public void addAction(string name, int startTime, int durationTime)
        {
            DataStructures.Action ACS = new DataStructures.Action(name, startTime, durationTime);
            actions.Add(ACS);
        }
        (List<DataStructures.Observation>, List<DataStructures.Action>) GetScenarios(int time)
        {
            List<DataStructures.Observation> reObservations = new List<DataStructures.Observation>();
            List<DataStructures.Action> retActions = new List<DataStructures.Action>();
            foreach (DataStructures.Observation obs in observations)
            {
                if (obs.Time == time)
                {
                    reObservations.Add(obs);
                }
            }
            foreach (DataStructures.Action acs in actions)
            {
                if (acs.StartTime == time)
                {
                    retActions.Add(acs);
                }
            }
            (List<DataStructures.Observation>, List<DataStructures.Action>) scenario = (reObservations, retActions);
            return scenario;
        }

        internal Observations GetObservations()
        {
            throw new NotImplementedException();
        }
    }
}
