﻿using KR_Lib;
using KR_Lib.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Action = KR_Lib.DataStructures.Action;
using KR_Lib.Statements;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Scenarios;
using System.Collections.Generic;
using KnowledgeRepresentationLib.Scenarios;

namespace KR_Tests
{
    /// <summary>
    /// Testy przykładów z wykładu z indykiem
    /// </summary>
    [TestClass]
    public class TurkeyTest
    {
        /*
         * Yale Shooting problem with changes
         * 
         * 
         * (load, 1) causes loaded
         * (load,1) invokes escape
         * (shoot,1) causes ¬loaded
         * (escape, 1) releases hidden
         * (shoot, 1) causes ¬alive if loaded and ¬hidden
         */

        #region Variables

        IEngine engine;

        Action load;
        Action shoot;
        Action escape;

        Fluent loaded;
        Fluent alive;
        Fluent hidden;

        IFormula loadedFormula;
        IFormula aliveFormula;
        IFormula hiddenFormula;

        IFormula negloadedFormula;
        IFormula negaliveFormula;
        IFormula neghiddenFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            load = new Action("load");
            engine.AddAction(load);

            shoot = new Action("shoot");
            engine.AddAction(shoot);

            escape = new Action("escape");
            engine.AddAction(escape);

            #endregion

            #region Add actions

            loaded = new Fluent("loaded");
            engine.AddFluent(loaded);

            alive = new Fluent("alive");
            engine.AddFluent(alive);

            hidden = new Fluent("hidden");
            engine.AddFluent(hidden);

            #endregion

            #region Add common formulas

            loadedFormula = new Formula(loaded);
            aliveFormula = new Formula(alive);
            hiddenFormula = new Formula(hidden);

            negloadedFormula = new NegationFormula(loadedFormula);
            negaliveFormula = new NegationFormula(aliveFormula);
            neghiddenFormula = new NegationFormula(hiddenFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(load, 1), loadedFormula));
            engine.AddStatement(new InvokeStatement(new ActionTime(load, 1), new ActionTime(escape, 1)));
            engine.AddStatement(new ReleaseStatement(new ActionTime(escape, 1), hidden, null));
            engine.AddStatement(new CauseStatement(new ActionTime(shoot, 1), negaliveFormula, new ConjunctionFormula(neghiddenFormula, loadedFormula)));
            engine.AddStatement(new CauseStatement(new ActionTime(shoot, 1), negloadedFormula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(¬loaded∧¬hidden∧alive,0), (¬loaded∧¬hidden∧¬alive,4)}
             * Acs={(load,1,1),(shoot,1,3)}
             * 
             * Kwerenda 1:
             * Czy scenariusz jest osiagany?
             * 
             * Odpowiedź 1:
             * Tak
             * 
             * Kwerenda 2:
             * Czy akcja escape dzieje sie w 2 chwili czasowej?
             * 
             * Odpowiedź 2:
             * Tak
             * 
             * Kwerenda 3:
             * Czy indyk żyje zawsze w czasie 4?
             * 
             * Odpowiedź 3:
             * Nie
             * 
             * Kwerenda 4:
             * Czy indyk żyje kiedykolwiek w czasie 4?
             * 
             * Odpowiedź 4:
             * Nie
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negloadedFormula, aliveFormula, neghiddenFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negloadedFormula, negaliveFormula, neghiddenFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 4) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(load, 1, 1), new ActionOccurrence(shoot, 1, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery posibleScenarioQuery = new PossibleScenarioQuery(scenario.Id);
            IQuery actionQuery = new ActionQuery(2, escape, scenario.Id);
            IQuery formulaQuery = new FormulaQuery(4, aliveFormula, scenario.Id, QueryType.Always);
            IQuery formulaQuery2 = new FormulaQuery(4, aliveFormula, scenario.Id, QueryType.Ever); 

            #endregion

            #region Testing
            engine.SetMaxTime(4);


            bool responsePosibleScenarioQuery = engine.ExecuteQuery(posibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeTrue();
            bool responseActionQuery = engine.ExecuteQuery(actionQuery);
            responseActionQuery.Should().BeTrue();
            bool responseFormulaQuery = engine.ExecuteQuery(formulaQuery);
            responseFormulaQuery.Should().BeFalse();
            bool responseFormulaQuery2 = engine.ExecuteQuery(formulaQuery2);
            responseFormulaQuery2.Should().BeFalse();
            //TODO: Wcześniej tu było BeTrue ale to nie jest zgodne z obserwacjami.
            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             * Obs={(¬loaded∧¬hidden∧alive,0), (¬loaded∧¬hidden∧¬alive,4)}
             * Acs={(load,1,1),(shoot,1,1)}
             * 
             * Kwerenda 1:
             * Czy scenariusz jest osiagany kiedykolwiek?
             * 
             * Odpowiedź 1:
             * Nie, mamy dwie współliniowe akcje w chwili czasowej 1.
             * 
             * Kwerenda 2:
             * Czy scenariusz jest osiagany zawsze?
             * 
             * Odpowiedź 1:
             * Nie, mamy dwie współliniowe akcje w chwili czasowej 1.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negloadedFormula, aliveFormula, neghiddenFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negloadedFormula, negaliveFormula, neghiddenFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario2")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 4) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(load, 1, 1), new ActionOccurrence(shoot, 1, 1) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery posibleScenarioQuery = new PossibleScenarioQuery(scenario.Id);
            IQuery posibleScenarioQuery2 = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(4);

            bool responsePosibleScenarioQuery = engine.ExecuteQuery(posibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeFalse();
            bool responsePosibleScenarioQuery2 = engine.ExecuteQuery(posibleScenarioQuery2);
            responsePosibleScenarioQuery2.Should().BeFalse();

            #endregion
        }

        [TestMethod]
        public void TestScenario3()
        {
            /*
             * Obs={(¬loaded∧¬hidden∧alive,0), (¬loaded∧¬hidden∧¬alive,0)}
             * Acs={(load,1,1),(shoot,1,3)}
             * 
             * Kwerenda 1:
             * Czy scenariusz jest osiagany kiedykolwiek?
             * 
             * Odpowiedź 1:
             * Nie, mamy dwie sprzeczne obserwacje w chwili 0.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negloadedFormula, aliveFormula, neghiddenFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negloadedFormula, negaliveFormula, neghiddenFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario3")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 0) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(load, 1, 1), new ActionOccurrence(shoot, 1, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery posibleScenarioQuery = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(4);

            bool responsePosibleScenarioQuery = engine.ExecuteQuery(posibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeFalse();

            #endregion
        }
    }
}
