using KR_Lib.Formulas;
using KR_Lib.Queries;
using KR_Lib.Structures;
using System;

namespace KR_Lib.Structures
{
    public interface IStructure
    {
        /// <summary>
        /// Czas trwania - do zakończenia działania ostatniej akcji
        /// </summary>
        int EndTime { get; }

        Structure ToModel();
        int H(Formula formula, int time);
        int E(Action action, int duration, int startTime);
        int CheckActionBelongingToE(Action action, int time);
        bool EvaluateFormula(Formula formula, int time);
    }
}
