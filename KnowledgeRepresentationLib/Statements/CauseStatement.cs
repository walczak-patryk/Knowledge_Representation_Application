using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private IFormula formulaCaused;
        private IFormula formulaIf;
        private bool ifFlag = false;

        public CauseStatement(ActionTime action, IFormula formulaCaused, IFormula formulaIf = null) : base(action)
        {
            this.formulaCaused = formulaCaused;
            if(formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }
        }

        private bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
            bool doFlag = false;
            // if działa aktualnie tylko z formula o wartości true
            if (ifFlag)
            {
                formulaIf.SetFluentsStates(fluents);
                doFlag = (currentAction == action && time - currentAction.StartTime == (action as ActionTime).Time && formulaIf.Evaluate());
            }
            else
                doFlag = (currentAction == action &&  time - currentAction.StartTime == (action as ActionTime).Time);

            return doFlag;
        }

        private void DoStatement(ref List<State> newStates)
        {                
            List<State> states = new List<State> ();       
            foreach (var s in newStates)
            {                           
                var combinations = formulaCaused.GetStatesFluents(true);
                foreach(var listOfFluents in combinations){
                    var state = new State(s.CurrentActions, s.Fluents.Select(f => (Fluent)f.Clone()).ToList(), s.ImpossibleActions, s.FutureActions);
                    foreach (Fluent fluent in listOfFluents)
                    {
                        state.Fluents.Find(f => f == fluent).State = fluent.State;
                    }
                    states.Add(state);
                }
            }
            newStates = states;
        }

        public override void CheckAndDo(State parentState, ref List<State> newStates, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                this.DoStatement(ref newStates);
            return;
        }
    }
}
