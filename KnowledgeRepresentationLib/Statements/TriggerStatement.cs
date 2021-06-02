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

        public void DoStatement(State state, int time)
        {
            ActionWithTimes actionWTimes = new ActionWithTimes(action, (action as ActionTime).Time, time);
            state.CurrentActions.Add(actionWTimes);
            
            return;
        }

        public override void CheckAndDo(State parentState, ref List<State> newStates, int time)
        {
            foreach(var state in newStates)
            {
                if(CheckStatement(state.CurrentActions.FirstOrDefault(), state.Fluents, state.ImpossibleActions, time))
                    this.DoStatement(state, time);           
            }
            return;
        }
    }
}

