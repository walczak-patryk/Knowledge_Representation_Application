using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Queries;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Structures
{
    public class Structure : IStructure
    {
        public int EndTime { get; }

        public List<ActionWithTimes> Acs { get; }

        public List<(int, List<Fluent>)> TimeFluents1 { get; set; }
        //or
        public Dictionary<int, List<Fluent>> TimeFluents2 { get; set; }

        public List<(Fluent, ActionWithTimes, int)> OcclusionRegions { get; set; }

        public List<(Action, int, int)> E { get; set; }

        public Structure(int endTime, List<ActionOccurrence> acs, List<ActionWithTimes> actions, List<(int, List<Fluent>)> timeFluents1 /*or Dictionary<int, List<Fluent>> TimeFluents2*/)
        {
            EndTime = endTime;
            Acs = acs;
			E = actions;
            TimeFluents1 = timeFluents1;
            //TimeFluents2 = timeFluents2;
            OcclusionRegions = new List<(Fluent, ActionWithTimes, int)>();
            for(int i = 0; i < EndTime; i++)
            {
                var fluents = O(null, i);
                if (fluents.Count == 0)
                    continue;
                else
                {
<<<<<<<<< Temporary merge branch 1
                    var action = E.Find(x => x.StartTime <= i && x.GetEndTime() > i);
=========
                    var action = Acs.Find(x => x.StartTime <= i && x.DurationTime + x.StartTime > i);
>>>>>>>>> Temporary merge branch 2
                    foreach (var item in fluents)
                        OcclusionRegions.Add((item, action, i));
                }
            }
        }

        public Structure ToModel()
        {
            return new Model();
        }

        public bool H(Formula formula, int time)
        {
            var timefluents = TimeFluents1.Find(x => x.Item1 < time);

            //or            

            //var timefluents = TimeFluents2[time];

            var formFluents = formula.GetFluents();

            foreach(var fl in formFluents)
            {
                var fluent = timefluents.Item2.Find(x => x.Id == fl.Id);
                fl.State = fluent.State;
            }
            
            return formula.Evaluate();
            
        }

        public List<Fluent> O(Action action, int time)
        {
            var result = new List<Fluent>();

            var startFluents = TimeFluents1.Find(x => x.Item1 < time);
            var endFluents = TimeFluents1[TimeFluents1.IndexOf(startFluents) + 1];
            if (endFluents.Item1 != time)
                return null;
            else
            {
                for(int i = 0; i < endFluents.Item2.Count; i++)
                {
                    if (endFluents.Item2[i].State != startFluents.Item2[i].State)
                        result.Add(endFluents.Item2[i]);
                }
            }

            //or

            //var endFluents = TimeFluents2[time];
            //var startFluents = TimeFluents2[time - 1];
            //for(int i = 0; i < endFluents.Count; i++)
            //{
            //    if (endFluents[i].State != startFluents[i].State)
            //        result.Add(endFluents[i]);
            //}

            return result;
        }

        public bool CheckActionBelongingToE(Action action, int time)
        {
            return E.Contains((action, action.DurationTime, time));
        }

        public bool EvaluateFormula(Formula formula, int time) // = H
        {
            return this.H(formula, time);
        }
    }
}
