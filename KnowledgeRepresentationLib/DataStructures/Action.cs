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
        }

        public Action(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
