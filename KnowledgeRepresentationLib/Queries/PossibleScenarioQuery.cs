﻿using System;
using System.Collections.Generic;
using KR_Lib.DataStructures;
using KR_Lib.Scenarios;
using KR_Lib.Structures;

namespace KR_Lib.Queries 
{
    /// <summary>
    /// Zapytanie użytkownika czy podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek
    /// </summary>
    public class PossibleScenarioQuery : IQuery
    {

        public QueryType QueryType 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Odpowiedź na pytanie czy podany scenariusz jest możliwy do realizacji zawsze/kiedykolwiek
        /// </summary>
        /// <param name="models">Lista modeli i niespójnych struktur</param>
        /// <param name="scenario">Scenariusz</param>
        /// <returns>bool</returns>
        public bool GetAnswer(List<IStructure> models, IScenario scenario)
        {
            bool atLeatOneModel = false;
            bool atLeatOneInconsistent = false;
            foreach (var model in models)
            {
                if (model is InconsistentStructure) atLeatOneInconsistent = true;
                else if (model is Model) atLeatOneModel = true;
            }
            if (this.QueryType == QueryType.Ever) return atLeatOneModel;
            else return !atLeatOneInconsistent;
        }
    }
}
