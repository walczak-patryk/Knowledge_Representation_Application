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
    /// Testy trzeciego przykładu z dokumentacji
    /// </summary>
    [TestClass]
    public class FoodTest
    {
        /*
        * Dwójka znajomych umówiła się na gotowanie wspólne lunchu oraz w lodówce mają tylko trzy jajka, 
        * szczypiorek ipomidory. Franek chce ugotować szakszukę a Ania zdecydowała, że zrobi jajecznicę. 
        * Oboje nie mogą robić swojegojedzenia w tym samym momencie, ponieważ każde z dań wymaga trzech 
        * jajek. Zrobienie którejkolwiek z potrawwymaga pójścia do sklepu. 
        * Ponadto, w kuchni dostępny jest tylko jeden palnik.
        * 
        * (making_szakszuka,3)causes¬jajka∧ ¬palnik∧szakszuka
        * (making_omllet,3)causes¬jajka∧ ¬palnik∧szakszuka
        * impossible making_szakszuka, making_omlet
        * (buy_eggs,4)causes eggs
         */

        #region Variables

        IEngine engine;

        Action makingSzakszuka;
        Action makingOmlet;
        Action buyEggs;

        Fluent jajka;
        Fluent omlet;
        Fluent szakszuka;
        Fluent palnik;

        IFormula jajkaFormula;
        IFormula omletFormula;
        IFormula palnikFormula;
        IFormula szakszukaFormula;

        IFormula negjajkaFormula;
        IFormula negomletFormula;
        IFormula negpalnikFormula;
        IFormula negszakszukaFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            makingSzakszuka = new Action("makingSzakszuka");
            engine.AddAction(makingSzakszuka);

            buyEggs = new Action("buyEggs");
            engine.AddAction(buyEggs);

            makingOmlet = new Action("makingOmlet");
            engine.AddAction(makingOmlet);

            #endregion

            #region Add actions

            jajka = new Fluent("jajka");
            engine.AddFluent(jajka);

            omlet = new Fluent("omlet");
            engine.AddFluent(omlet);

            szakszuka = new Fluent("szakszuka");
            engine.AddFluent(szakszuka);

            palnik = new Fluent("palnik");
            engine.AddFluent(palnik);

            #endregion

            #region Add common formulas

            jajkaFormula = new Formula(jajka);
            omletFormula = new Formula(omlet);
            szakszukaFormula = new Formula(szakszuka);
            palnikFormula = new Formula(palnik);

            negjajkaFormula = new NegationFormula(jajkaFormula);
            negomletFormula = new NegationFormula(omletFormula);
            negszakszukaFormula = new NegationFormula(szakszukaFormula);
            negpalnikFormula = new NegationFormula(palnikFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(makingSzakszuka, 3), negjajkaFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(makingSzakszuka, 3), negpalnikFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(makingSzakszuka, 3), szakszukaFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(makingOmlet, 3), negjajkaFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(makingOmlet, 3), negpalnikFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(makingOmlet, 3), omletFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(buyEggs, 1), jajkaFormula));
            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs= (¬jajka∧palnik∧szakszuka, 4)
             * Acs:{(making_szakszuka,3,0),(making_omllet,3,4)}
             * 
             * Kwerenda:
             * Czy w chwili 4 scenariusza wykonywana jest akcja making_omllet?
             * 
             * Odpowiedź:
             * W chwili 4 nie jest wykonywana żadna obserwacja ze względu na brak dostępnych jajek
             */

            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negjajkaFormula, negomletFormula, szakszukaFormula, palnikFormula);
           
            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() {new Observation(observationFormula1, 4) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(makingSzakszuka, 3, 0), new ActionOccurrence(makingOmlet, 3, 4) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(4, makingOmlet, scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(5);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeFalse();
            // Zwraca błąd, niespójny opis z dodanymi danymi w domenie (impossible)

            #endregion
        }
    }
}

