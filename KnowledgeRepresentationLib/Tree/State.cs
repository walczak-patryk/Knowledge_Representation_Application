
using System;
using System.Collections.Generic;
using System.Linq;
using KR_Lib.DataStructures;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Tree
{
    public class State : ICloneable
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

        public List<ActionWithTimes> FutureActions
        {
            get;
            set;
        }

        public bool InvalidDescription
        {
            get;
            set;
        }

        public State(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions)
        {
            this.CurrentActions = currentActions;
            this.Fluents = fluents;
            this.ImpossibleActions = impossibleActions;
            this.FutureActions = futureActions;
            this.InvalidDescription = false;
        }

        public object Clone()
        {
            return new State(CurrentActions, Fluents.Select(f => (Fluent)f.Clone()).ToList(), ImpossibleActions, FutureActions);
        }

    }
}
