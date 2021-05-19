using KR_Lib.DataStructures;

namespace KnowledgeRepresentationLib.Scenarios
{
    public class ActionOccurrences : ActionWithTimes
    {
        public ActionOccurrences(string name, int durationTime, int startTime) : base(name, durationTime, startTime)
        { }

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
