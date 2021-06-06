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

        public Action(Action action)
        {
            Id = action.Id;
            Name = action.Name;
        }
        public Action(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return "(" + this.Name + ")";
        }
        public static bool operator ==(Action obj1, Action obj2)
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

        public static bool operator !=(Action obj1, Action obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Action other)
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
            return Equals(obj as Action);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }
    }
}
