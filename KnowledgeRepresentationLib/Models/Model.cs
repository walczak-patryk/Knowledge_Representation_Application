using System;
using System.Collections.Generic;
using KR_Lib.Formulas;

namespace KR_Lib.Models
{
    public class Structure : IStructure
    {
        public List<(Action,int)> E
        {
            get;
            set;
        }

        public Structure ToModel()
        {
            throw new NotImplementedException();
        }

        public int H(Formula formula, int time)
        {
            throw new NotImplementedException();
        }
    }
}
