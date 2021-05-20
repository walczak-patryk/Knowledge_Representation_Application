using System;

namespace KR_Lib.DataStructures
{
    public class ActionTime : Action
    {
        public int Time
        {
            get;
            set;
        }
      
        public ActionTime() { }
        public ActionTime(Action action, int time) : base(action.Name)
        {
            this.Time = time;
        }
    }
}
