﻿using KR_Lib.Scenarios;
using KR_Lib.Structures;
using System;
using System.Collections.Generic;

namespace KR_Lib.Queries
{
    /// <summary>
    /// Zapytanie użytkownika czy w chwili t realizacji scenariusza wykonywana jest akcja A
    /// </summary>
    public class ActionQuery : IQuery
    {
        int time;
        Action action;

        public Action Action
        {
            get
            {
                return action;
            }
        }
        public int Time
        {
            get
            {
                return time;
            }
        }

        /// <summary>
        /// Odpowiedź na pytanie czy w chwili t realizacji scenariusza wykonywana jest akcja A
        /// </summary>
        /// <param name="models">Lista modeli i niespójnych struktur</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>bool</returns>
        public bool GetAnswer(List<IStructure> models, IScenario scenario)
        {
            foreach (var model in models)
            {
                if (model.CheckActionBelongingToE(this.Action, this.Time) != 1)
                    return false;
            }
            return true;
        }
    }
}
