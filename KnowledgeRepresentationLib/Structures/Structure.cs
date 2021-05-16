using System;
using System.Collections.Generic;
using KR_Lib.Formulas;

namespace KR_Lib.Structures
{
    public class Structure : IStructure
    {
        //public List<(Action, int)> E
        //{
        //    get; set;
        //}

        public int EndTime { get; }

        public List<Action> Actions { get; }

        public Structure ToModel()
        {
            throw new NotImplementedException();
        }

        public bool H(Formula formula, int time)
        {
            throw new NotImplementedException();
        }

        public int E(Action action, int duration, int startTime)
        {
            throw new NotImplementedException();
        }

        public int CheckActionBelongingToE(Action action, int time)
        {
            throw new NotImplementedException();
        }

        public bool EvaluateFormula(Formula formula, int time) // = H
        {
            throw new NotImplementedException();
        }
    }
}
