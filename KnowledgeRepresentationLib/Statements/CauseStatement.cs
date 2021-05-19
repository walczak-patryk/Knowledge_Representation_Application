using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Tree;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class CauseStatement : Statement
    {
        private IFormula formulaCaused;
        private IFormula formulaIf;
        bool ifFlag = false;

        public CauseStatement(Action action, IFormula formulaCaused, IFormula formulaIf = null) : base(action)
        {
            this.formulaCaused = formulaCaused;
            if(formulaIf != null)
            {
                ifFlag = true;
                this.formulaIf = formulaIf;
            }
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
