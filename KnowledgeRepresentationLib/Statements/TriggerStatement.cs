using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Statements
{
    public class TriggerStatement : Statement
    {
        IFormula formulaIf;

        public TriggerStatement(ActionTime action, IFormula formulaIf) : base(action)
        {
            this.formulaIf = formulaIf;
        }

        public bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
            formulaIf.SetFluentsStates(fluents);
            return formulaIf.Evaluate();
        }

        public List<(State, HashSet<Fluent>)> DoStatement(State state, int time)
        {
            ActionWithTimes actionWTimes = new ActionWithTimes(action, (action as ActionTime).Time, time);
            state.CurrentActions.Add(actionWTimes);
            
            return new List<(State, HashSet<Fluent>)>() { (state, null) };
        }

        public override List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time)
        {
            if(CheckStatement(newState.CurrentActions.FirstOrDefault(), newState.Fluents, newState.ImpossibleActions, time))
                return this.DoStatement(newState, time);           
            return null;
        }
    }
}

