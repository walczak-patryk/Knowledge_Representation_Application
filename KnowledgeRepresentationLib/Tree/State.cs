using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;

namespace KR_Lib.Tree
{
    public class State
    {

        DataStructures.Action currentAction;
        List<Fluent> Fluents
        {
            get;
            set;               
        }

        public State(DataStructures.Action currentAction, List<Fluent> fluents)
        {
            this.currentAction = currentAction;
            this.Fluents = fluents;
        }

    }
}
