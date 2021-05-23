using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ImpossibleAtStatement : Statement
    {
        private int time;

        public ImpossibleAtStatement(Action action, int time) : base(action)
        {
            this.time = time;
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            return time == currentTime;
        }

        public override State DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions)
        {
            var actionWTime = (action as ActionWithTimes);
            impossibleActions.Add(actionWTime);
            return new State(currentActions, fluents, impossibleActions);
        }
    }
}
