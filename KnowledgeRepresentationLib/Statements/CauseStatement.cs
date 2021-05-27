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
        private bool doFlag = false;
        bool ifFlag = false;

        public CauseStatement(ActionTime action, IFormula formulaCaused, IFormula formulaIf = null) : base(action)
        {
            this.formulaCaused = formulaCaused;
            if(formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int time)
        {
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

        public override List<State> DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions, int time)
        {     
            List<State> states = new List<State> ();       
            if (doFlag)
            {                           
                var combinations = formulaCaused.GetStatesFluents(true);
                foreach(var listOfFluents in combinations){
                    var state = new State(currentActions, fluents.Select(f => (Fluent)f.Clone()).ToList(), impossibleActions, futureActions);
                    foreach (Fluent fluent in listOfFluents)
                    {
                        state.Fluents.Find(f => f == fluent).State = fluent.State;
                    }
                    states.Add(state);
                }
                return states;
            }
            else{
                states.Add(new State(currentActions, fluents, impossibleActions, futureActions));
            }

            return states;
        }

        public override bool GetDoFlag()
        {
            return doFlag;
        }

    }
}
