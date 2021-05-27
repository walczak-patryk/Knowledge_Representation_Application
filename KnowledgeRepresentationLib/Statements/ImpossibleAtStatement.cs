using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ImpossibleAtStatement : Statement
    {
        private int time;
        private bool doFlag = false;

        public ImpossibleAtStatement(Action action, int time) : base(action)
        {
            this.time = time;
        }

        public override bool CheckStatement(ActionWithTimes currentAction, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, int currentTime)
        {
            doFlag = time == currentTime;

            return doFlag;
        }

        public override List<State> DoStatement(List<ActionWithTimes> currentActions, List<Fluent> fluents, List<ActionWithTimes> impossibleActions, List<ActionWithTimes> futureActions, int time)
        {
            if (doFlag)
            {
                var actionWTime = (action as ActionWithTimes);
                impossibleActions.Add(actionWTime);
            }

            return new List<State>() { new State(currentActions, fluents, impossibleActions, futureActions) };
        }

        public override bool GetDoFlag()
        {
            return doFlag;
        }

    }
}
