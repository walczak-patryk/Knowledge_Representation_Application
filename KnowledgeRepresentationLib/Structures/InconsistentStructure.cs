using System;
using System.Collections.Generic;
using KR_Lib.Exceptions;
using KR_Lib.Formulas;

namespace KR_Lib.Structures
{
    public class InconsistentStructure : IStructure
    {
        public Structure ToModel()
        {
            throw new InconsistentException();
        }

        public int H(Formula formula, int time)
        {
            throw new InconsistentException();
        }
    }
}
