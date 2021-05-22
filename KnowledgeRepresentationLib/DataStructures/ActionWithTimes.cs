using System;

namespace KR_Lib.DataStructures
{
    public class ActionWithTimes : Action, ICloneable
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
            this.Name = name;
            this.StartTime = startTime;
            this.DurationTime = durationTime;
        }

        protected ActionWithTimes(Action action, int durationTime, int startTime) : base(action.Name)
        {
            this.Name = action.Name;
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

        /// <summary>
        /// Checks if Action in this object takes place in given time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CheckIfActiveAt(int time)
        {
            if (time >= this.StartTime && time < this.GetEndTime())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            //string description = "Action (" + Id + ", " + Duration + ") with start time: " + StartAt;
            //return description;
            return "(" + this.Name + ", " + this.DurationTime + ")";
        }

        public object Clone()
        {
            return new ActionWithTimes(this.Name, this.DurationTime, this.StartTime);
        }
    }
}
