using System;

namespace KR_Lib.DataStructures
{
    public class Action
    {
        public Guid Id
        {
            get;
        }
        public string Name
        {
            get;
            set;
        }
      
        public Action() { }
        public Action(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid();
        }

        public bool Equals(Action action)
        {
            if (action.Id.Equals(this.Id))
                return true;
            return false;
        }

        public override string ToString()
        {
            return "(" + this.Name + ")";
        }
    }
}
