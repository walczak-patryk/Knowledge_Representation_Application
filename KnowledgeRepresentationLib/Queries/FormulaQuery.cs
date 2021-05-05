using System;
namespace KR_Lib.Queries
{
    public class FluentQuery : IQuery
    {
        int time;
        Action action;

        public FluentQuery()
        {
        }

        public Action Action
        {
            get
            {
                return action;
            }
        }

        public int Time
        {
            get
            {
                return time;
            }
        }
    }
}
