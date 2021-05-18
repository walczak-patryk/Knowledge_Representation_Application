using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Lib.DataStructures
{
    public class ScenarioItem
    {
        public Guid Id { get; set; }
        public string Moment { get; set; }
        public string ActionOccurence { get; set; }
        public (Action, int) ActionDetails { get; set; }
        public string Observation { get; set; }
        public string Duration { get; set; }

        public ScenarioItem(string Moment, string ActionOccurence, string Observation, string Duration)
        {
            this.Id = Guid.NewGuid();
            this.Moment = Moment;
            this.ActionOccurence = ActionOccurence;
            this.Observation = Observation;
            this.Duration = Duration;
        }
    }
}
