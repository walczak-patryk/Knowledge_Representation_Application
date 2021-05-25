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

        public static bool operator ==(Fluent obj1, Fluent obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(Fluent obj1, Fluent obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Fluent other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Fluent);
        }
    }
}
