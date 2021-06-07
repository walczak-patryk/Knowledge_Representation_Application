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
    /// Testy drugiego przyjkładu z dokumentacji
    /// </summary>
    [TestClass]
    public class TravelTest
    {
        /*
         * Julia jedzie do swojej rodziny na wakacje. Zawsze jeżeli długo śpi zanim wyjedzie, musi jechać szybko żeby zdążyć
         * na obiad. Jeżeli jedzie szybko w momencie kiedy wyskoczy jej na drogę jeleń i droga hamowania jest za długa, 
         * uderzy w jelenia co będzie skutkowało jego śmiercią. 
         * 
         * (sleep,10) causes late 
         * (sleep,8) causes ¬late
         * (driving_slow,8) causes arrived
         * (driving_slow,8) causes ¬late
         * (driving_fast,6) causes arrived
         * (driving_fast,1) causes deer_dead if deer_on_road
         * (driving_fast,1) causes ¬deer_on_road if deer_on_road
         */

        #region Variables

        IEngine engine;

        Action sleep;
        Action drivingSlow;
        Action drivingFast;

        Fluent late;
        Fluent arrived;
        Fluent deerOnRoad;
        Fluent deerDead;

        IFormula lateFormula;
        IFormula arrivedFormula;
        IFormula deerDeadFormula;
        IFormula deerOnRoadFormula;

        IFormula negLateFormula;
        IFormula negArrivedFormula;
        IFormula negDeerDeadFormula;
        IFormula negDeerOnRoadFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            sleep = new Action("sleep");
            engine.AddAction(sleep);

            drivingFast = new Action("drivingFast");
            engine.AddAction(drivingFast);

            drivingSlow = new Action("drivingSlow");
            engine.AddAction(drivingSlow);

            #endregion

            #region Add fluents

            late = new Fluent("late");
            engine.AddFluent(late);

            arrived = new Fluent("arrived");
            engine.AddFluent(arrived);

            deerOnRoad = new Fluent("deerOnRoad");
            engine.AddFluent(deerOnRoad);

            deerDead = new Fluent("deerDead");
            engine.AddFluent(deerDead);

            #endregion

            #region Add common formulas

            lateFormula = new Formula(late);
            arrivedFormula = new Formula(arrived);
            deerOnRoadFormula = new Formula(deerOnRoad);
            deerDeadFormula = new Formula(deerDead);

            negLateFormula = new NegationFormula(lateFormula);
            negArrivedFormula = new NegationFormula(arrivedFormula);
            negDeerOnRoadFormula = new NegationFormula(deerOnRoadFormula);
            negDeerDeadFormula = new NegationFormula(deerDeadFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(sleep, 10), lateFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(sleep, 8), negLateFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(drivingSlow, 8), arrivedFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(drivingSlow, 8), negLateFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(drivingFast, 6), arrivedFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(drivingFast, 6), negLateFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(drivingFast, 1), deerDeadFormula, deerOnRoadFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(drivingFast, 1), negDeerOnRoadFormula, deerOnRoadFormula));
            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(¬late∧¬arrived∧ ¬dead_deer∧ ¬deer_on_road,0), (¬late∧arrived∧ ¬dead_deer∧ ¬deer_on_road,14)}
             * Acs:{(sleep,6,0),(driving_slow,8,6)}
             * 
             * Kwerenda:
             * Czy w chwili 5 scenariusza wykonywana jest akcja sleep?
             * 
             * 
             * Odpowiedź:
             * W tym przypadku w chwili 5 wykonywana akcja sleep.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negLateFormula, negArrivedFormula, negDeerOnRoadFormula, negDeerDeadFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negLateFormula, arrivedFormula, negDeerOnRoadFormula, negDeerDeadFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 14) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(sleep, 6, 0), new ActionOccurrence(drivingSlow, 8, 6) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(5, sleep, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(15);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();
            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             *Obs={(¬late∧ ¬arrived∧ ¬dead_dear∧ ¬dear_on_road,0),(¬late∧arrived∧ ¬dead_dear∧¬dear_on_road,14)}
             *Acs:{(sleep,6,0),(driving_slow,8,6)}
             *
             *Kwerenda:Czy w chwili 5 scenariusza wykonywana jest akcja driving_slow?
             *
             *Odpowiedź:W tym przypadku w chwili 5 nie wykonywana akcja driving_slow.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negLateFormula, negArrivedFormula, negDeerOnRoadFormula, negDeerDeadFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negLateFormula, arrivedFormula, negDeerOnRoadFormula, negDeerDeadFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario2")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 14) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(sleep, 6, 0), new ActionOccurrence(drivingSlow, 8, 6) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(5, drivingSlow, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(15);
            bool response = engine.ExecuteQuery(query);
            //TODO: jest tylko jedna i w dodatku inconsistent structure. Ale najpierw trzeba uspójnić domenę z opisem u góry
            response.Should().BeFalse();

            #endregion
        }

        [TestMethod]
        public void TestScenario3()
        {
            /*
             * Obs={(¬late∧ ¬arrived∧ ¬dead_dear∧ ¬dear_on_road,0),¬late∧arrived∧dead_dear∧¬dear_on_road,14)}
             * Acs:{(sleep,8,0),(driving_fast,6,8)}
             * 
             * Kwerenda:Czy w chwili5scenariusza wykonywana jest akcjadriving_slow?
             * 
             * Odpowiedź:W tym przypadku w chwili5 nie jest wykonywanadriving_slow. 
             * Mimo, że ta akcja nie jest wogóle wykonywana w danym scenariuszu, nadal możemy zapytać o jej wykonanie w danej chwili czasowejscenariusza.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negLateFormula, negArrivedFormula, negDeerOnRoadFormula, negDeerDeadFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negLateFormula, arrivedFormula, negDeerOnRoadFormula, negDeerDeadFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario3")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 14) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(sleep, 8, 0), new ActionOccurrence(drivingFast, 6, 8) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(5, drivingSlow, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(15);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeFalse();

            #endregion
        }
    }
}
