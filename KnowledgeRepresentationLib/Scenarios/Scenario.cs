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
        Guid Id { get; }
        int GetScenarioDuration();
        (List<DataStructures.Observation>, List<DataStructures.Action>) GetScenarios(int time);
        List<DataStructures.Observation> GetObservationsAtTime(int time);
        DataStructures.Action GetActionAtTime(int time);
        //List<DataStructures.Observation> observations { get; set; }
        //List<DataStructures.Action> actions { get; set; }
    }
    public class Scenario : IScenario
    {
        public string Name { get; set; }
        public Guid Id { get; }
        public List<DataStructures.Observation> observations { get; set; }
        public List<DataStructures.Action> actions { get; set; }
        public Scenario() { }
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
        public (List<DataStructures.Observation>, List<DataStructures.Action>) GetScenarios(int time)
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
                if (acs.StartTime <= time && acs.StartTime + acs.DurationTime >= time)
                {
                    retActions.Add(acs);
                }
            }
            (List<DataStructures.Observation>, List<DataStructures.Action>) scenario = (reObservations, retActions);
            return scenario;
        }

        public List<DataStructures.Observation> GetObservationsAtTime(int time)
        {
            List<DataStructures.Observation> observations = new List<DataStructures.Observation>();
            foreach (DataStructures.Observation obs in observations)
            {
                if (obs.Time == time)
                {
                    observations.Add(obs);
                }
            }

            return observations;
        }

        public DataStructures.Action GetActionAtTime(int time)
        {
            DataStructures.Action action = null;
            foreach (DataStructures.Action acs in actions)
            {
                if (acs.StartTime <= time && acs.StartTime + acs.DurationTime >= time)
                {
                    action = acs;
                    break;
                }
            }

            return action;
        }

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
    }
}