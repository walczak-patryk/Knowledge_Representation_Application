using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {
        public ReleaseStatement(Action action, Fluent fluent, Formula formula) : base(action, fluent, formula)
        {
            fluent.State = true;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int currentTime)
        {
            return (currentAction.GetEndTime() == currentTime && formula.Evaluate() == true) ;
        }

        public override State DoStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions)
        {
            // zmiana stanu fluentu na przeciwny
            fluents.Find(f => f.Name.Equals(fluent.Name)).State = !fluents.Find(f => f.Name.Equals(fluent.Name)).State;
            return new State(currentAction, fluents, impossibleActions);
        }
    }
}
