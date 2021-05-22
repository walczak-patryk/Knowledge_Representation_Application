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

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int currentTime)
        {
            return time == currentTime;
        }

        public override State DoStatement(List<Action> currentActions, List<Fluent> fluents, List<Action> impossibleActions)
        {
            impossibleActions.Add(action);
            return new State(currentActions, fluents, impossibleActions);
        }
    }
}
