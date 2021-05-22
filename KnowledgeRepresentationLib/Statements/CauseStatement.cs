using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private IFormula formulaCaused;
        private IFormula formulaIf;
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
                return currentAction == action && formulaIf.Evaluate() == true;
            }

            return currentAction == action;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions)
        {
            var currentAction = currentActions[0];
            // zmiana stanu fluentu na przeciwny
            if (!(currentAction is ActionTime))
            {
                throw new Exception("Niewłaściwy typ w CauseStatement: potrzebny ActionTime");
            }
            foreach (Fluent fluent in formulaCaused.GetFluents())
            {
                fluents.Find(f => f.Name.Equals(fluent.Name)).State = !fluents.Find(f => f.Name.Equals(fluent.Name)).State;
            }
            return new State(currentActions, fluents, impossibleActions);
        }

    }
}
