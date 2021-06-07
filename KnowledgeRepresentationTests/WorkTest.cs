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
    /// Testy pierwszego przyjkładu z dokumentacji
    /// </summary>
    [TestClass]
    public class WorkTest
    {
        /*
         * Katarzyna tworzy oprogramowanie dla firmy w której pracuje. Jeżeli uda jej się szybko dokończyć oprogramowanie
         * przez ciężką pracę dostanie bonus do pensji. Z pieniędzy z bonusu będzie mogła kupić sobie nowego laptopa jeżeli
         * pójdzie na zakupy z otrzymanymi pieniędzmi.
         * 
         * (hard_working,8) causes ¬work
         * (hard_working,8) causes bonus
         * (shopping,1) causes laptop if bonus
         */

        #region Variables

        IEngine engine;

        Action hardWorking;
        Action shopping;

        Fluent work;
        Fluent bonus;
        Fluent laptop;

        IFormula workFormula;
        IFormula bonusFormula;
        IFormula laptopFormula;
        IFormula negWorkFormula;
        IFormula negBonusFormula;
        IFormula negLaptopFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            hardWorking = new Action("hard_working");
            engine.AddAction(hardWorking);

            shopping = new Action("shopping");
            engine.AddAction(shopping);

            #endregion

            #region Add actions

            work = new Fluent("work");
            engine.AddFluent(work);

            bonus = new Fluent("bonus");
            engine.AddFluent(bonus);

            laptop = new Fluent("laptop");
            engine.AddFluent(laptop);

            #endregion

            #region Add common formulas

            workFormula = new Formula(work);
            bonusFormula = new Formula(bonus);
            laptopFormula = new Formula(laptop);

            negWorkFormula = new NegationFormula(workFormula);
            negBonusFormula = new NegationFormula(bonusFormula);
            negLaptopFormula = new NegationFormula(laptopFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(hardWorking, 8), negWorkFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(hardWorking, 8), bonusFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(shopping, 1), laptopFormula, bonusFormula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(work∧¬bonus∧¬laptop,0),(¬work∧bonus∧laptop,9)}
             * Acs:{(hard_working,8,0),(shopping,2,8)}
             * 
             * Kwerenda:
             *  Czy dany scenariusz jest do realizacji zawsze?
             *  
             * Odpowiedź:
             *  W tym przypadku scenariusz jest zgodny z dziedziną oraz jest zawsze realizowany.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(workFormula, negBonusFormula, negLaptopFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negWorkFormula, bonusFormula, laptopFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 9) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(hardWorking, 8, 0), new ActionOccurrence(shopping, 2, 8) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(10);
            bool responsePosibleScenarioQuery = engine.ExecuteQuery(query);
            responsePosibleScenarioQuery.Should().BeTrue();

            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             * Obs={(work∧¬bonus∧¬laptop,0),(¬work∧¬bonus∧¬laptop,9)}
             * Acs:{(hard_working,8,0)}
             * 
             * Kwerenda:
             *  Czy dany scenariusz jest do realizacji zawsze?
             *  
             * Odpowiedź:
             *  W tym przypadku scenariusz jest zgodny z dziedziną oraz jest zawsze realizowany.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(workFormula, negBonusFormula, negLaptopFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negWorkFormula, bonusFormula, negLaptopFormula);

            #endregion

            #region Add scenarios


            IScenario scenario = new Scenario("TestScenario2")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 9) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(hardWorking, 8, 0) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(10);
            var responsePosibleScenarioQuery = engine.ExecuteQuery(query);
            responsePosibleScenarioQuery.Should().BeTrue();
            #endregion
        }

        [TestMethod]
        public void TestScenario3()
        {
            /*
             * Obs={(work∧¬bonus∧¬laptop,0),(¬work∧¬bonus∧¬laptop,3)}
             * Acs:{(hard_working,8,0),(shopping,2,8)}
             * 
             * Kwerenda:
             *  Czy dany scenariusz jest do realizacji kiedykolwiek?
             *  
             * Odpowiedź:
             *  W tym przypadku scenariusz jest niezgodny z dziedziną, gdyż nie ma takiego układu akcji które
             *  będą się działy tak aby dane obserwacje mogły być zrealizowane gdyż obserwacja wymaga krótszego czasu niż
             *  zrealizowanie tego stanu w przypadku obserwacji pierwszej gdzie stan jest inny. Więc nie jest on do realizacji
             *  kiedykolwiek.
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(workFormula, negBonusFormula, negLaptopFormula);
            IFormula observationFormula2 = new ConjunctionFormula(negWorkFormula, negBonusFormula, negLaptopFormula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("TestScenario3")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 3) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(hardWorking, 8, 0), new ActionOccurrence(shopping, 2, 8) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(10);
            var responsePosibleScenarioQuery = engine.ExecuteQuery(query);
            responsePosibleScenarioQuery.Should().BeFalse();
            #endregion
        }
    }
}
