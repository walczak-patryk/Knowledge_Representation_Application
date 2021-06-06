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
    /// Testy czwartego przyjkładu z dokumentacji
    /// </summary>
    [TestClass]
    public class FriendsTest
    {
        /*
         * Pewnego wieczoru Krystyna poszła się spotkać z przyjaciółkami, 
         * jej rodzice zawsze proszą ją żeby, ze względówbezpieczeństwa, była w domu przed 23:00. 
         * Podczas spotkania koleżanki oglądały film, Krystyna nie patrzyła na zegareki dopiero po 
         * jakimś czasie zorientowała się jak późno jest. 
         * Pójście pieszo zajmie jej dużo czasu. 
         * Ponieważ odjechał jejtramwaj, wtedy, żeby zdążyć do domu przed ustalonym czasem jest 
         * zmuszona pojechać taksówką.
         * 
         * 
         * (watching,3)causes¬tram
         * (watching,2)causes late
         * (commute,2)impossible if¬money
         * (commute,3) causes angry_dad
         * (commute,2) causes ¬money∧ ¬late if ¬tram
         * (commute,2) causes ¬late if tram
         */

        #region Variables

        IEngine engine;

        Action watching;
        Action commute;

        Fluent late;
        Fluent tram;
        Fluent money;
        Fluent angryDad;

        IFormula lateFormula;
        IFormula tramFormula;
        IFormula angryDadFormula;
        IFormula moneyFormula;

        IFormula negLateFormula;
        IFormula negtramFormula;
        IFormula negangryDadFormula;
        IFormula negmoneyFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            watching = new Action("watching");
            engine.AddAction(watching);

            commute = new Action("commute");
            engine.AddAction(commute);

            #endregion

            #region Add fluents

            late = new Fluent("late");
            engine.AddFluent(late);

            tram = new Fluent("tram");
            engine.AddFluent(tram);

            money = new Fluent("money");
            engine.AddFluent(money);

            angryDad = new Fluent("angryDad");
            engine.AddFluent(angryDad);

            #endregion

            #region Add common formulas

            lateFormula = new Formula(late);
            tramFormula = new Formula(tram);
            moneyFormula = new Formula(money);
            angryDadFormula = new Formula(angryDad);

            negLateFormula = new NegationFormula(lateFormula);
            negtramFormula = new NegationFormula(tramFormula);
            negmoneyFormula = new NegationFormula(moneyFormula);
            negangryDadFormula = new NegationFormula(angryDadFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(watching, 3), negtramFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(watching, 2), lateFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(commute, 3), angryDadFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(commute, 8), negLateFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(commute, 2), negLateFormula, negtramFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(commute, 2), negmoneyFormula, tramFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(commute, 2), negLateFormula, tramFormula));
            engine.AddStatement(new ImpossibleIfStatement(new ActionTime(commute, 2), negmoneyFormula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(¬late∧¬angry_dad∧money,0)}
             * Acs={(watching,3,0),(commute,3,3)}
             * 
             * Kwerenda:
             * Czy osiągalny jest stan money∧¬angry_dad dla zadanego zbioru obserwacji?
             * 
             * Odpowiedź:
             * Stan money∧¬angry_dadjest osiągalny w czasie 0. 
             * Później w scenariuszu ujęto akcję commute trwającą 3, która powoduje rozzłoszczenie taty.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negLateFormula, negangryDadFormula, moneyFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(watching, 3, 0), new ActionOccurrence(commute, 3, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new TargetQuery(new ConjunctionFormula(moneyFormula, negangryDadFormula), QueryType.Ever, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(8);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();
            //TODO: błędne sprawdzanie stanów zamiast fluentów

            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             *Obs={(¬late∧¬angry_dad∧money,0)}
             *Acs={(watching,2,0),(commute,2,2)}
             *
             *Kwerenda:
             *Czy osiągalny jest stan money∧¬angry_dad dla zadanego zbioru obserwacji?
             *
             *Odpowiedź:
             *Stanmoney∧¬angry_dadosiągalny jest w czasie 0 i w czasie 4
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negLateFormula, negangryDadFormula, moneyFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario2")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(watching, 2, 0), new ActionOccurrence(commute, 2, 2) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new TargetQuery(new ConjunctionFormula(moneyFormula, negangryDadFormula), QueryType.Ever, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(8);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            #endregion
        }
    }
}
