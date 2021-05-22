using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KnowledgeRepresentationLib.Scenarios;
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

        //?
        /// <summary>
        /// Akcje - potrzebne do wyliczenia E
        /// </summary>
        List<ActionOccurrence> Acs { get; }

        /// <summary>
        /// Stany fluentów w czasie t - potrzebne do funkcji historii
        /// </summary>
        List<(int, List<Fluent>)> TimeFluents1 { get; set; }
        //or
        Dictionary<int, List<Fluent>> TimeFluents2 { get; set; }

        /// <summary>
        /// Regiony okluzji dla struktury
        /// </summary>
        List<(Fluent, ActionWithTimes, int)> OcclusionRegions { get; set; }

        /// <summary>
        /// Relacja E
        /// </summary>
        List<ActionWithTimes> E { get; set; }


        Structure ToModel();
        bool H(Formula formula, int time);
        List<Fluent> O(Action action, int time);
        bool CheckActionBelongingToE(KR_Lib.DataStructures.Action action, int time);
        bool EvaluateFormula(Formula formula, int time);
    }
}
