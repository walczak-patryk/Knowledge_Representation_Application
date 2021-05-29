using System;
using System.Collections.Generic;
using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Queries;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Structures
{
    public class Structure : IStructure
    {
        #region Properties
        public int EndTime { get; set; }

        public List<ActionOccurrence> Acs { get; set; }

        public Dictionary<int, List<Fluent>> TimeFluents { get; set; }

        public List<(Fluent, ActionWithTimes, int)> OcclusionRegions { get; set; }

        public List<ActionWithTimes> E { get; set; }

        #endregion

        #region Methods

        public Structure(int endTime)
        {
            this.EndTime = endTime;
            Acs = new List<ActionOccurrence>();
            E = new List<ActionWithTimes>();
            TimeFluents = new Dictionary<int, List<Fluent>>();
        }

        public Structure(Structure original)
        {
            this.EndTime = original.EndTime;

            Acs = new List<ActionOccurrence>();
            foreach (var ac in original.Acs)
                Acs.Add((ActionOccurrence)ac.Clone());

            E = new List<ActionWithTimes>();
            foreach (var e in original.E)
                E.Add((ActionWithTimes)e.Clone());

            TimeFluents = new Dictionary<int, List<Fluent>>();
            foreach(KeyValuePair<int, List<Fluent>> entry in original.TimeFluents)
            {
                var list = new List<Fluent>();
                foreach (var fluent in entry.Value)
                    list.Add((Fluent)fluent.Clone());
                TimeFluents[entry.Key] = list;
            }
        }

        public void FinishStructure()
        {
            OcclusionRegions = new List<(Fluent, ActionWithTimes, int)>();
            for (int i = 0; i < EndTime; i++)
            {
                foreach(var actionE in E)
                {
                    var fluents = O(actionE, i);
                    if (fluents.Count == 0)
                        continue;
                    else
                    {
                        foreach (var item in fluents)
                            OcclusionRegions.Add((item, actionE, i));
                    }
                }
            }
        }

        public Structure ToModel()
        {
            return new Model(EndTime);
        }

        public bool H(IFormula formula, int time)
        {           
            var timefluents = TimeFluents[time];

            var formFluents = formula.GetFluents();

            foreach(var fl in formFluents)
            {
                var fluent = timefluents.Find(x => x.Id == fl.Id);
                fl.State = fluent.State;
            }
            
            return formula.Evaluate();
            
        }

        public List<Fluent> O(ActionWithTimes action, int time)
        {
            if (action.StartTime > time || action.GetEndTime() < time)
                return null;

            var result = new List<Fluent>();

            var startFluents = TimeFluents[time - 1];
            var endFluents = TimeFluents[time];

            for (int i = 0; i < endFluents.Count; i++)
            {
                if (endFluents[i].State != startFluents[i].State)
                    result.Add(endFluents[i]);
            }

            return result;
        }

        public bool CheckActionBelongingToE(Action action, int time)
        {
            var result = E.FindAll(x => x.Id == action.Id);
            return result.Count > 0;
        }

        public bool EvaluateFormula(IFormula formula, int time) // = H
        {
            return this.H(formula, time);
        }

        #endregion
    }
}
