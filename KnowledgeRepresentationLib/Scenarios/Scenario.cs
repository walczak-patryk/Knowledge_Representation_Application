using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Scenarios
{
    public interface IScenario
    {
        string Name { get; set; }
        Guid Id { get; }

        int GetScenarioDuration();
        (List<Observation>, List<DataStructures.Action>) GetScenarios(int time);
        List<Observation> GetObservationsAtTime(int time);
        List<ActionWithTimes> GetStartingActions(int time);
        void AddObservation(Observation observation);
        void RemoveObservation(Guid observationId);
        void AddActionOccurrence(ActionOccurrence actionOccurrence);
        void RemoveActionOccurrence(Guid actionOccurrenceId);
    }
    public class Scenario : IScenario
    {
        public string Name { get; set; }
        public Guid Id { get; }
        public List<Observation> Observations
        {
            get;
            set;
        }
        public List<ActionOccurrence> ActionOccurrences
        {
            get;
            set;
        }
        public Scenario() { }
        public Scenario(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid();
            this.Observations = new List<Observation>();
            this.ActionOccurrences = new List<ActionOccurrence>();
        }
        public void addObservation(string name, IFormula formula, int time)
        {
            Observation OBS = new Observation(formula, time);
            Observations.Add(OBS);
        }
        public void RemoveObservation(Guid observationId)
        {
            Observations.RemoveAll((elem) => elem.ObservationId == observationId);
        }
        public void AddActionOccurrence(ActionOccurrence actionOccurrence)
        {
            ActionOccurrences.Add(actionOccurrence);
        }
        public void RemoveActionOccurrence(Guid actionOccurrenceId)
        {
            ActionOccurrences.RemoveAll((elem) => elem.ActionOccurenceId == actionOccurrenceId);

        }

        public void addAction(string name, int startTime, int durationTime)
        {
            var actionOccurrence = new ActionOccurrence(name, startTime, durationTime);
            ActionOccurrences.Add(actionOccurrence);
        }

        public (List<Observation>, List<DataStructures.Action>) GetScenarios(int time)
        {
            List<Observation> reObservations = new List<Observation>();
            List<DataStructures.Action> retActions = new List<DataStructures.Action>();
            foreach (Observation obs in Observations)
            {
                if (obs.Time == time)
                {
                    reObservations.Add(obs);
                }
            }
            foreach (ActionWithTimes acs in ActionOccurrences)
            {
                if (acs.StartTime <= time && acs.StartTime + acs.DurationTime >= time)
                {
                    retActions.Add(acs);
                }
            }
            (List<Observation>, List<DataStructures.Action>) scenario = (reObservations, retActions);
            return scenario;
        }

        public List<Observation> GetObservationsAtTime(int time)
        {
            return Observations.Where(obs => obs.Time == time).ToList();
        }

        public List<Observation> GetObservations()
        {
            return Observations;
        }
        public int GetScenarioDuration()
        {
            int durationObs = 0;
            int durationAcs = 0;
            foreach (ActionWithTimes acs in ActionOccurrences)
            {
                durationAcs += acs.DurationTime;
            }
            foreach (Observation obs in Observations)
            {
                durationObs = Math.Max(durationObs, obs.Time);
            }
            return Math.Max(durationAcs, durationObs);
        }

        public List<ActionWithTimes> GetStartingActions(int time)
        {
            List<ActionWithTimes> startingActions = new List<ActionWithTimes>();
            foreach (var action in ActionOccurrences)
            {
                var actionT = new ActionWithTimes(action);
                if (actionT.StartTime == time)
                {
                    startingActions.Add(actionT);
                }
            }

            return startingActions;
        }

        public void AddObservation(Observation observation)
        {
            this.Observations.Add(observation);
        }
    }
}