using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {

        private Fluent fluent;
        private IFormula formulaIf;
        bool ifFlag = false;

        public ReleaseStatement(Action action, Fluent fluent, IFormula formulaIf) : base(action)
        {
            this.fluent = fluent;
            if (formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }
        }

        public bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            if (action != currentAction)
            {
                return false;
            }

            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                return (currentTime - currentAction.StartTime == (action as ActionTime).Time && formulaIf.Evaluate());
            }
            
            return (currentTime - currentAction.StartTime == (action as ActionTime).Time);
        }

        public List<(State, bool)> DoStatement(State newState)
        {
            var s = new State(newState.CurrentActions, newState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), newState.ImpossibleActions, newState.FutureActions);
            s.Fluents.Find(f => f == fluent).State = !s.Fluents.Find(f => f == fluent).State;
            
            return new List<(State, bool)>() {(s,true)};
        }
        
        public override List<(State, bool)> CheckAndDo(State parentState, State newState, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                return this.DoStatement(newState);
            return null;
        }

    }
}
