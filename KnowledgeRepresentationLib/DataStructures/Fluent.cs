using System;

namespace KR_Lib.DataStructures
{
    public class Fluent : ICloneable
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

        private Fluent(string name, bool initialState, Guid guid)
        {
            this.Name = name;
            this.State = initialState;
            this.Id = guid;
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Fluent(Name, State, Id);
        }

        public bool Equals(Fluent fluent)
        {
            if (fluent.Id.Equals(this.Id))
                return true;
            return false;
        }
    }
}
