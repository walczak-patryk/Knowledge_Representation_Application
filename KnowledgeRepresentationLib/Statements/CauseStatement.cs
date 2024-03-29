﻿using KR_Lib.DataStructures;
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

        private List<(State, HashSet<Fluent>)> DoStatement(State newState)
        {                
            List<(State, HashSet<Fluent>)> states = new List<(State, HashSet<Fluent>)>();       

            var combinations = formulaCaused.GetStatesFluents(true);
            HashSet<Fluent> affectedFluents = formulaCaused.GetFluents().ToHashSet();
            foreach(var listOfFluents in combinations){
                var state = new State(newState.CurrentActions, newState.Fluents.Select(f => (Fluent)f.Clone()).ToList(), newState.ImpossibleActions, newState.FutureActions);
                HashSet<Fluent> set = new HashSet<Fluent>();
                foreach (Fluent fluent in listOfFluents)
                {
                    state.Fluents.Find(f => f == fluent).State = fluent.State;
                    set.Add(fluent);
                }
                states.Add((state, set));
            }

            return states;
        }

        public override List<(State, HashSet<Fluent>)> CheckAndDo(State parentState, State newState, int time)
        {
            if(CheckStatement(parentState.CurrentActions.FirstOrDefault(), parentState.Fluents, parentState.ImpossibleActions, time))
                return this.DoStatement(newState);
            return null;
        }
    }
}
