using KR_Lib.DataStructures;

namespace KnowledgeRepresentationLib.Scenarios
{
    public class ActionOccurrence : ActionWithTimes
    {
        public ActionOccurrence(string name, int durationTime, int startTime) : base(name, durationTime, startTime)
        { }

        public ActionOccurrence(Action action, int durationTime, int startTime) : base(action, durationTime, startTime)
        { }
    }
}
