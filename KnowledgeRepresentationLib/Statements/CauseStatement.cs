using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private Formula formulaCaused;

        public CauseStatement(Action action, Fluent fluent, Formula formula, Formula formulaCaused) : base(action, null, formula)
        {
            this.formulaCaused = formulaCaused;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, int time)
        {
            // if działa aktualnie tylko z formula o wartości true
            if (currentAction == action && formula.Evaluate() == true)
            {
                return true;
            }

            return false;
        }

        public override State DoStatement(Action currentAction, List<Fluent> fluents)
        {
            // zmiana stanu fluentu na przeciwny
            fluents.Find(f => f.Name.Equals(formulaCaused.fluent.Name)).State = !fluents.Find(f => f.Name.Equals(formulaCaused.fluent.Name)).State;
            return new State(currentAction, fluents);
        }

    }
}
