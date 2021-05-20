using System;

namespace KR_Lib.DataStructures
{
    public class ActionWithTimes : Action
    {
        public int StartTime
        {
            get;
            set;
        }
        public int DurationTime
        {
            get;
            set;
        }
        private ActionWithTimes() { }
        public ActionWithTimes(string name, int durationTime, int startTime) : base(name)
        {
            this.StartTime = startTime;
            this.DurationTime = durationTime;
        }

        protected ActionWithTimes(Action action, int durationTime, int startTime) : base(action)
        {
            this.StartTime = startTime;
            this.DurationTime = durationTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is ActionWithTimes)
            {
                var action = obj as ActionWithTimes;
                if (action.Id.Equals(this.Id) && action.DurationTime.Equals(this.DurationTime))
                    return true;
            }
            return false;
        }

        public int GetEndTime()
        {
            int time = -1;
            int? endTime = StartTime + DurationTime;

            if (endTime.HasValue)
            {
                time = endTime.Value;
            }

            return time;
        }

        public override string ToString()
        {
            //string description = "Action (" + Id + ", " + Duration + ") with start time: " + StartAt;
            //return description;
            return "(" + this.Name + ", " + this.DurationTime + ")";
        }
    }
}
