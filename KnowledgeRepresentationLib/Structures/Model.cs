using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Structures;
using System.Collections.Generic;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Queries
{
    public class Model : Structure
    {
        public Model(int endTime, List<ActionOccurrence> acs, List<(Action, int, int)> actions, List<(int, List<Fluent>)> timeFluents1) : base(endTime, acs, actions, timeFluents1)
        {

        }

        public new Structure ToModel()
        {
            return this as Model;
        }
    }
}