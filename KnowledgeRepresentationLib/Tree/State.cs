
using System.Collections.Generic;
using KR_Lib.DataStructures;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Tree
{
    public class State
    {

        public List<Action> CurrentActions
        {
            get;
            set;
        }
        public List<Fluent> Fluents
        {
            get;
            set;               
        }

        public List<Action> ImpossibleActions
        {
            get;
            set;
        }

        public State(List<Action> currentActions, List<Fluent> fluents, List<Action> impossibleActions)
        {
            this.CurrentActions = currentActions;
            this.Fluents = fluents;
            this.ImpossibleActions = impossibleActions;
        }

    }
}
