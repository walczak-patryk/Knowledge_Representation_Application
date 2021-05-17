using System;

namespace KnowledgeRepresentationLib.Scenarios
{
    public class ActionOccurrences
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public KR_Lib.DataStructures.Action Act { get; set; }
        public int Time { get; set; }
        public ActionOccurrences(string name, KR_Lib.DataStructures.Action act, int time) : base()
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Act = act;
            this.Time = time;
        }

        /// <summary>
        /// Checks if Action in this object takes place in given time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CheckIfActiveAt(int time)
        {
            if (time >= this.Act.StartTime && time < this.Act.GetEndTime())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override string ToString()
        {
            return "Action " + this.Act.ToString() + " in time " + Time.ToString();
        }
    }
}
