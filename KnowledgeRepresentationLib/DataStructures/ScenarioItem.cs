using KnowledgeRepresentationLib.Scenarios;
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
        public ActionOccurrence ActionOccurence_engine { get; set; }
        public string Observation { get; set; }
        public string Duration { get; set; }
        public List<ObservationElement> observationElements { get; set; }
        public int Moment_int { get; set; }
        public int Duration_int { get; set; }

        public ScenarioItem(string ActionOccurence,Action Action, int Moment_int, int Duration_int, string Observation, List<ObservationElement> observationElements)
        {
            this.Id = Guid.NewGuid();
            this.Moment = Moment.ToString();
            this.ActionOccurence = ActionOccurence;
            this.ActionOccurence_engine = new ActionOccurrence(Action, Duration_int, Moment_int);
            this.Observation = Observation;
            this.Duration = Duration.ToString();
            this.observationElements = observationElements;
            this.Moment_int = Moment_int;
            this.Duration_int = Duration_int;
        }
    }
}
