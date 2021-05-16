using System;

namespace KR_Lib.DataStructures
{
    public class Action 
    {
        public Guid Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
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
        public Action() { }
        public Action(string name, int startTime, int durationTime)
        {
            this.Name = name;
            this.StartTime = startTime;
            this.DurationTime = durationTime;
            this.Id = Guid.NewGuid();
        }
        public override bool Equals(object obj)
        {
            if (obj is Action)
            {
                var action = obj as Action;
                if (action.Id.Equals(this.Id) && action.DurationTime.Equals(this.DurationTime))
                    return true;
            }
            return false;
        }
        public object Clone()
        {
            Action Act = new Action();
            Act.Name = Name;
            Act.Id = Id;
            Act.DurationTime = DurationTime;
            Act.StartTime = StartTime;

            return Act;
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
