using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using Action = KR_Lib.DataStructures.Action;
using System;
using System.Collections.Generic;

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

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            if (!(currentAction is ActionWithTimes))
            {
                throw new Exception("Niewłaściwy typ w ReleaseStatement: potrzebny ActionWithTimes");
            }
            var actionWithTimes = (currentAction as ActionWithTimes);

            if (ifFlag)
            {
                return actionWithTimes.GetEndTime() == currentTime && formulaIf.Evaluate() == true;
            }

            return actionWithTimes.GetEndTime() == currentTime;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions)
        {
            // zmiana stanu fluentu na przeciwny
            fluents.Find(f => f.Name.Equals(fluent.Name)).State = !fluents.Find(f => f.Name.Equals(fluent.Name)).State;
            return new State(currentActions, fluents, impossibleActions);
        }

    }
}
