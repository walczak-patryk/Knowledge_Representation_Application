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

        public List<(State, HashSet<Fluent>)> DoStatement(State newState)
        {
            var s = new State(newState.CurrentActions, newState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), newState.ImpossibleActions, newState.FutureActions);
            var f2 = fluent.Clone() as Fluent;
            f2.State = !fluent.State;
            s.Fluents.Find(f => f == fluent).State = f2.State;
            newState.Fluents.Find(f => f == fluent).State = fluent.State;
            
            return new List<(State, HashSet<Fluent>)>() { (newState, new HashSet<Fluent>() { fluent }), (s, new HashSet<Fluent>() { f2 })  };
        }
        
        public override List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                return this.DoStatement(newState);
            return null;
        }

    }
}
