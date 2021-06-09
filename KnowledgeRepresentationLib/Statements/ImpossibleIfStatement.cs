using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Statements
{
    public class ImpossibleIfStatement : Statement
    {
        IFormula formulaIf;
        private bool ifFlag = false;

        public ImpossibleIfStatement(Action action, IFormula formulaIf = null) : base(action)
        {
            if (formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }       
        }

        public bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                return formulaIf.Evaluate();
            }
            return true;
        }

        public List<(State, HashSet<Fluent>)> DoStatement(State newState)
        {
            var a = newState.ImpossibleActions.Where(act => act == action).SingleOrDefault();
            if (a == null)
            {
                var actionTime = (action as ActionTime);
                var actionWTime = new ActionWithTimes(action, actionTime.Time, -1);
                newState.ImpossibleActions.Add(actionWTime);
            }
               
            return new List<(State, HashSet<Fluent>)>() {(newState, null)};
        }
        
        public override List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time)
        {
            if(CheckStatement(newState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                return this.DoStatement(newState);
            return null;
        }
    }
}
