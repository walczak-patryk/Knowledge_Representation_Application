using System;

namespace KR_Lib.DataStructures
{
    public class Fluent
    {
        public Guid Id
        {
            get;
        }

        public bool State
        { 
            get;
            set;
        }

        public string Name 
        { 
            get;
        }

        public Fluent(string name, bool initialState = false)
        {
            this.Name = name;
            this.State = initialState;
            this.Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
