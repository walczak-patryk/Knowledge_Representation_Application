using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Tree
{
    public class State
    {

        public DataStructures.ActionWithTimes currentAction;
        public List<Fluent> Fluents
        {
            get;
            set;               
        }

        public List<DataStructures.ActionWithTimes> impossibleActions
        {
            get;
            set;
        }

        public State(DataStructures.ActionWithTimes currentAction, List<Fluent> fluents, List<DataStructures.ActionWithTimes> impossibleActions)
        {
            this.currentAction = currentAction;
            this.Fluents = fluents;
            this.impossibleActions = impossibleActions;
        }

    }
}
