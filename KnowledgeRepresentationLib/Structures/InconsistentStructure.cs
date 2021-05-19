using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Structures;
using System.Collections.Generic;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Structures
{
    public class InconsistentStructure : Structure
    {
        public InconsistentStructure(int endTime, List<ActionOccurrence> acs, List<(Action, int, int)> actions, List<(int, List<Fluent>)> timeFluents1) : base(endTime, acs, actions, timeFluents1)
        {

        }

        public new Structure ToModel()
        {
            return this as InconsistentStructure;
        }
    }
}
