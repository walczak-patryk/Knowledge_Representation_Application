using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Tree
{
    public class State
    {

        public List<DataStructures.Action> currentActions
        {
            get;
            set;
        }
        public List<Fluent> Fluents
        {
            get;
            set;               
        }

        public List<DataStructures.Action> impossibleActions
        {
            get;
            set;
        }

        public State(List<DataStructures.Action> currentActions, List<Fluent> fluents, List<DataStructures.Action> impossibleActions)
        {
            this.currentActions = currentActions;
            this.Fluents = fluents;
            this.impossibleActions = impossibleActions;
        }

    }
}
