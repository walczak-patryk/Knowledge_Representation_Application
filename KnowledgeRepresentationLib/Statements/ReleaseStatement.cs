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

        public void DoStatement(ref List<State> newStates)
        {
            List<State> states = new List<State>();
            foreach (var state in newStates)
            {
                var s = new State(state.CurrentActions, state.Fluents.Select(f => (Fluent)f.Clone()).ToList(), state.ImpossibleActions, state.FutureActions);
                s.Fluents.Find(f => f == fluent).State = !s.Fluents.Find(f => f == fluent).State;
                states.Add(s);
            }
            newStates.AddRange(states);
            
            return;
        }
        
        public override void CheckAndDo(State parentState, ref List<State> newStates, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                this.DoStatement(ref newStates);
            return;
        }

    }
}
