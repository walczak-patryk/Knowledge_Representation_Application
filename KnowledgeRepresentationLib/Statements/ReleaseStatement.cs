using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System.Collections.Generic;

namespace KR_Lib.Statements
{
    public class ReleaseStatement : Statement
    {
        public ReleaseStatement(Action action, Fluent fluent, Formula formula) : base(action, fluent, formula)
        {
            fluent.State = true;
        }

        public override bool CheckStatement(Action currentAction, List<Fluent> fluents, int time)
        {
            if (currentAction.GetEndTime() <= time && formula.Evaluate() == true)
            {
                return true;
            }

            return false;
        }

        public List<Fluent> DoStatement(Action currentAction, List<Fluent> fluents)
        {
            // zmiana stanu fluentu na przeciwny
            fluents.Find(f => f.Name.Equals(fluent.Name)).State = !fluents.Find(f => f.Name.Equals(fluent.Name)).State;
            return fluents;
        }
    }
}
