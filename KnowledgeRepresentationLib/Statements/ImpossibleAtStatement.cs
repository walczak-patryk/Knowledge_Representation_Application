using KR_Lib.DataStructures;
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

        public void DoStatement(State newState)
        {
            var actionWTime = (action as ActionWithTimes);
            newState.ImpossibleActions.Add(actionWTime);
            return;
        }

        public override void CheckAndDo(State parentState, ref List<State> newStates, int time)
        {   
            foreach( var newState in newStates)
            {
                if(CheckStatement(newState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                    this.DoStatement(newState);
            }
            return;
        }

    }
}
