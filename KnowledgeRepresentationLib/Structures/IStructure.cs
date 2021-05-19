using KR_Lib.Formulas;
using System.Collections.Generic;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Lib.Structures
{
    public interface IStructure
    {
        /// <summary>
        /// Czas trwania - do zakończenia działania ostatniej akcji
        /// </summary>
        int EndTime { get; }

        /// <summary>
        /// Akcje - potrzebne do wyliczenia E
        /// </summary>
        List<Action> Actions { get; }

        Structure ToModel();
        bool H(Formula formula, int time);
        int E(Action action, int duration, int startTime);
        int CheckActionBelongingToE(Action action, int time);
        bool EvaluateFormula(Formula formula, int time);
    }
}
