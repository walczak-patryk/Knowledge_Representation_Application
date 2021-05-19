using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private Formula formulaCaused;

        public CauseStatement(Action action, Fluent fluent, Formula formulaCaused, Formula formula = null) : base(action, null, formula)
        {
            this.formulaCaused = formulaCaused;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions, int time)
        {
            // if działa aktualnie tylko z formula o wartości true
            return currentAction == action && formula.Evaluate() == true;
        }

        public override State DoStatement(Action currentAction, List<Fluent> fluents, List<Action> impossibleActions)
        {
            // zmiana stanu fluentu na przeciwny
            fluents.Find(f => f.Name.Equals(formulaCaused.fluent.Name)).State = !fluents.Find(f => f.Name.Equals(formulaCaused.fluent.Name)).State;
            return new State(currentAction, fluents, impossibleActions);
        }

    }
}
