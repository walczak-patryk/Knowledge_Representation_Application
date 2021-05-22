using KnowledgeRepresentationLib.Scenarios;
using KR_Lib.DataStructures;
using KR_Lib.Structures;
using System.Collections.Generic;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Structures
{
    public class InconsistentStructure : Structure
    {
        public InconsistentStructure(int endTime) : base(endTime)
        {

        }

        public new Structure ToModel()
        {
            return this as InconsistentStructure;
        }
    }
}
