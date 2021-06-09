using System;
using System.Collections.Generic;
using System.Linq;
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
            for (int i = 0; i <= EndTime; i++)
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

        public virtual Structure ToModel()
        {
            FinishStructure();

            bool modelCheck = true;

            foreach (var occ in OcclusionRegions)
            {
                if (!(H(new Formula((occ.Item1.Clone() as Fluent)), occ.Item3) != H(new Formula((occ.Item1.Clone() as Fluent)), occ.Item3 - 1)))
                    modelCheck = false;
            }

            if (modelCheck)
                return new Model(this);
            else
                return new InconsistentStructure();
        }

        public bool H(IFormula formula, int time)
        {
            if (!TimeFluents.ContainsKey(time))
                return false;

            var timefluents = TimeFluents[time];
            formula.SetFluentsStates(timefluents);
            return formula.Evaluate();
            
        }

        public List<Fluent> O(ActionWithTimes action, int time)
        {
            var result = new List<Fluent>();

            if (!(action.StartTime < time && action.GetEndTime() >= time))
                return result;

            if (!TimeFluents.ContainsKey(time))
                return result;

            if (time == 0)
                return result;

            if (action.GetEndTime() < time)
                return result;

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
            var result = E.Where(x => x == action).FirstOrDefault();
            if(result != null){
                if(time >= result.StartTime && time < result.GetEndTime())
                    return true;
            }
            return false;    
        }

        public bool EvaluateFormula(IFormula formula, int time) // = H
        {
            return this.H(formula, time);
        }

        #endregion
    }
}
