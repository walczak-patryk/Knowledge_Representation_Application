﻿using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Statements
{
    public class ImpossibleAtStatement : Statement
    {
        private int time;

        public ImpossibleAtStatement(Action action, int time) : base(action)
        {
            this.time = time;
        }

        public bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            return (time == currentTime);
        }

        public List<(State, bool)> DoStatement(State newState)
        {
            var actionWTime = (action as ActionWithTimes);
            newState.ImpossibleActions.Add(actionWTime);
            return new List<(State, bool)>() {(newState, false)};
        }

        public override List<(State, bool)> CheckAndDo(State parentState, State newState, int time)
        {   
            if(CheckStatement(newState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                return this.DoStatement(newState);
            return null;
        }

    }
}
