using KR_Lib.DataStructures;
using System;
using Action = KR_Lib.DataStructures.Action;

namespace KnowledgeRepresentationLib.Scenarios
{
    public class ActionOccurrence : ActionWithTimes, ICloneable
    {
        public Guid ActionOccurenceId { get; set; }
        public ActionOccurrence(string name, int durationTime, int startTime) : base(name, durationTime, startTime)
        { }

        public ActionOccurrence(Guid ActionOccurenceId, string name, int durationTime, int startTime) : base(name, durationTime, startTime)
        {
            this.ActionOccurenceId = ActionOccurenceId;
        }

        public ActionOccurrence(Action action, int durationTime, int startTime) : base(action, durationTime, startTime)
        { }

    }
}
