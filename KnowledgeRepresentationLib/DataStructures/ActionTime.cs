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
        public ActionTime(string name, int time) : base(name)
        {
            this.Time = time;
        }
    }
}
