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

        public Fluent(string name, bool initialState)
        {
            this.Name = name;
            this.State = initialState;
        }
    }
}
