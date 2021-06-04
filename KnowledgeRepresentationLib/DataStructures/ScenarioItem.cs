using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.Formulas;
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
        public IFormula formula { get; set; }
        public int Moment_int { get; set; }
        public int Duration_int { get; set; }
        public List<ObservationElement> observationElements { get; set; }

        public ScenarioItem(string ActionOccurence,Action Action, int Moment_int, int Duration_int, string Observation, IFormula formula, List<ObservationElement> observationElements)
        {
            this.Id = Guid.NewGuid();
            this.Moment = Moment_int.ToString();
            this.ActionOccurence = ActionOccurence;
            if (Action == null)
            {
                this.ActionOccurence_engine = null;
            }
            else
            {
                this.ActionOccurence_engine = new ActionOccurrence(Action, Duration_int, Moment_int);
                this.ActionOccurence_engine.ActionOccurenceId = this.Id;
            }
            this.Observation = Observation;
            this.Duration = Duration_int.ToString();
            this.formula = formula;
            this.Moment_int = Moment_int;
            this.Duration_int = Duration_int;
            this.observationElements = observationElements;
        }
    }
}
