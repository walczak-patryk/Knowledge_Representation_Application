
using System.Collections.Generic;
using KR_Lib.DataStructures;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Tree
{
    public class State
    {

        public List<ActionWithTimes> CurrentActions
        {
            get;
            set;
        }
        public List<Fluent> Fluents
        {
            get;
            set;               
        }

        public List<ActionWithTimes> ImpossibleActions
        {
            get;
            set;
        }

        public State(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions)
        {
            this.CurrentActions = currentActions;
            this.Fluents = fluents;
            this.ImpossibleActions = impossibleActions;
        }

    }
}
