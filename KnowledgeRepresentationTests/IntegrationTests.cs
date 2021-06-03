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
    public class IntegrationTest
    {
        /*
         * Integration tests for different scenarios
         * 
         * 
         * (A, 1) releses f
         */

        #region Variables

        IEngine engine;

        Action A;

        Fluent f;
        Fluent g;

        IFormula fFormula;
        IFormula gFormula;

        IFormula negfFormula;
        IFormula neggFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            A = new Action("A");
            engine.AddAction(A);

            #endregion

            #region Add actions

            f = new Fluent("f");
            engine.AddFluent(f);

            g = new Fluent("g");
            engine.AddFluent(g);

            #endregion

            #region Add common formulas

            fFormula = new Formula(f);
            gFormula = new Formula(g);

            negfFormula = new NegationFormula(fFormula);
            neggFormula = new NegationFormula(gFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new ReleaseStatement(new ActionTime(A, 1), f, fFormula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(¬f,2), (¬f,5)}
             * Acs={(A,1,3)}
             * 
             * Kwerenda 1:
             * Czy scenariusz jest osiagany zawsze?
             * 
             * Odpowiedź 1:
             * Nie
             * 
             * Kwerenda 2:
             * Czy scenariusz jest osiagany kiedykolwiek?
             * 
             * Odpowiedź 2:
             * Tak
             * 
             * Kwerenda 3:
             * Czy ¬f w chwili 5 zawsze?
             * 
             * Odpowiedź 3:
             * Nie
             * 
             * Kwerenda 4:
             * Czy ¬f w chwili 5 kiedykolwiek?
             * 
             * Odpowiedź 4:
             * Tak
             * 
             * Kwerenda 5:
             * Czy ¬f w chwili 6 zawsze?
             * 
             * Odpowiedź 5:
             * Nie
             * 
             * Kwerenda 6:
             * Czy ¬f w chwili 6 kiedykolwiek?
             * 
             * Odpowiedź 6:
             * Tak
             */

            #region Add specific formulas

            IFormula observationFormula1 = negfFormula;
            IFormula observationFormula2 = negfFormula;

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0), new Observation(observationFormula2, 5) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(A, 1, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery posibleScenarioQuery = new PossibleScenarioQuery(QueryType.Always, scenario.Id);
            IQuery posibleScenarioQuery2 = new PossibleScenarioQuery(QueryType.Ever, scenario.Id);
            IQuery formulaQuery = new FormulaQuery(5, negfFormula, scenario.Id);
            IQuery formulaQuery2 = new FormulaQuery(6, negfFormula, scenario.Id);

            #endregion

            #region Testing
            engine.SetMaxTime(7);

            bool responsePosibleScenarioQuery = engine.ExecuteQuery(posibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeFalse();
            bool responsePosibleScenarioQuery2 = engine.ExecuteQuery(posibleScenarioQuery2);
            responsePosibleScenarioQuery2.Should().BeTrue();
            bool responseFormulaQuery = engine.ExecuteQuery(formulaQuery);
            responseFormulaQuery.Should().BeTrue();
            bool responseFormulaQuery2 = engine.ExecuteQuery(formulaQuery2);
            responseFormulaQuery2.Should().BeTrue();

            #endregion
        }
    }
}
