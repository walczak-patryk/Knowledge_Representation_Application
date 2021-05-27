using KR_Lib;
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
    /// Testy przykładów z wykładu
    /// </summary>
    [TestClass]
    public class LectureTest
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
            engine.AddStatement(new ReleaseStatement(new ActionTime(escape, 1), hidden, hiddenFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(shoot, 1), negaliveFormula, new ConjunctionFormula(neghiddenFormula, loadedFormula)));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(¬loaded∧¬hidden∧alive,0), (¬loaded∧¬hidden∧¬alive,4)}
             * Acs={(load,1,0),(shoot,1,3)}
             * 
             * Kwerenda:
             * Czy scenariusz jest osiagany zawsze
             * 
             * Odpowiedź:
             * Tak
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negloadedFormula, aliveFormula, neghiddenFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negloadedFormula, negaliveFormula, neghiddenFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 4) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(load, 1, 0), new ActionOccurrence(shoot, 1, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new PossibleScenarioQuery(QueryType.Ever, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(8);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             * Obs={(¬loaded∧¬hidden∧alive,0), (¬loaded∧¬hidden∧¬alive,0)}
             * Acs={(load,1,0),(shoot,3,1)}
             * 
             * Kwerenda:
             * Czy akcja escape dzieje sie w 2 chwili czasowej?
             * 
             * Odpowiedź:
             * Tak
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negloadedFormula, aliveFormula, neghiddenFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negloadedFormula, negaliveFormula, neghiddenFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario2")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 4) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(load, 1, 0), new ActionOccurrence(shoot, 1, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(3, escape, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(8);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            #endregion
        }
    }
}
