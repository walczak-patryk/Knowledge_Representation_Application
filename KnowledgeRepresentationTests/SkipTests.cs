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
    /// Testy przykładów z oddania, skip
    /// </summary>
    [TestClass]
    public class SkipTest
    {
        /*
         * Skip tests for different from handing over the project
         * 
         * 
         * (skip, 1) causes (a or not a)
         * (fire, 1) causes (l and not a)
         * (fire, 1) causes (l or not l)
         */

        #region Variables

        IEngine engine;

        Action skip;
        Action fire;

        Fluent a;
        Fluent l;

        IFormula aFormula;
        IFormula lFormula;

        IFormula negaFormula;
        IFormula neglFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            skip = new Action("skip");
            engine.AddAction(skip);

            fire = new Action("fire");
            engine.AddAction(fire);

            #endregion

            #region Add actions

            a = new Fluent("a");
            engine.AddFluent(a);

            l = new Fluent("l");
            engine.AddFluent(l);

            #endregion

            #region Add common formulas

            aFormula = new Formula(a);
            lFormula = new Formula(l);

            negaFormula = new NegationFormula(aFormula);
            neglFormula = new NegationFormula(lFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(skip, 1), new AlternativeFormula(aFormula, negaFormula)));
            engine.AddStatement(new CauseStatement(new ActionTime(fire, 1), new ConjunctionFormula(lFormula, negaFormula)));
            engine.AddStatement(new CauseStatement(new ActionTime(fire, 1), new AlternativeFormula(lFormula, neglFormula)));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             * Obs={(a,0)}
             * Acs={(skip,1,1), (fire,1,3)}
             * 
             * Kwerenda 1:
             * Czy scenariusz jest osiagany zawsze?
             * 
             * Odpowiedź 1:
             * Tak
             * 
             * Kwerenda 2:
             * Czy scenariusz jest osiagany kiedykolwiek?
             * 
             * Odpowiedź 2:
             * Tak
             * 
             * Kwerenda 3:
             * Czy ¬a w chwili 4 zawsze?
             * 
             * Odpowiedź 3:
             * Tak
             * 
             * Kwerenda 4:
             * Czy ¬a w chwili 4 kiedykolwiek?
             * 
             * Odpowiedź 4:
             * Tak
             * 
             * Kwerenda 5:
             * Czy ¬l w chwili 4 zawsze?
             * 
             * Odpowiedź 5:
             * Nie
             * 
             * Kwerenda 6:
             * Czy ¬l w chwili 4 kiedykolwiek?
             * 
             * Odpowiedź 6:
             * Nie
             */

            #region Add specific formulas

            IFormula observationFormula1 = aFormula;

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0)  },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(skip, 1, 1), new ActionOccurrence(fire, 1, 3) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery posibleScenarioQuery = new PossibleScenarioQuery(QueryType.Always, scenario.Id);
            IQuery posibleScenarioQuery2 = new PossibleScenarioQuery(QueryType.Ever, scenario.Id);
            IQuery formulaQuery = new FormulaQuery(4, negaFormula, scenario.Id, QueryType.Always);
            IQuery formulaQuery2 = new FormulaQuery(4, negaFormula, scenario.Id, QueryType.Ever);
            IQuery formulaQuery3 = new FormulaQuery(4, neglFormula, scenario.Id, QueryType.Always);
            IQuery formulaQuery4 = new FormulaQuery(4, neglFormula, scenario.Id, QueryType.Ever);

            #endregion

            #region Testing
            engine.SetMaxTime(5);

            bool responsePosibleScenarioQuery = engine.ExecuteQuery(posibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeTrue();
            bool responsePosibleScenarioQuery2 = engine.ExecuteQuery(posibleScenarioQuery2);
            responsePosibleScenarioQuery2.Should().BeTrue();
            bool responseFormulaQuery = engine.ExecuteQuery(formulaQuery);
            responseFormulaQuery.Should().BeTrue();
            bool responseFormulaQuery2 = engine.ExecuteQuery(formulaQuery2);
            responseFormulaQuery2.Should().BeTrue();
            bool responseFormulaQuery3 = engine.ExecuteQuery(formulaQuery3);
            responseFormulaQuery3.Should().BeFalse();
            bool responseFormulaQuery4 = engine.ExecuteQuery(formulaQuery4);
            responseFormulaQuery4.Should().BeFalse();

            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             * Obs={(a,5)}
             * Acs={}
             * 
             * Kwerenda 1:
             * Czy scenariusz jest osiagany zawsze?
             * 
             * Odpowiedź 1:
             * Tak
             * 
             * Kwerenda 2:
             * Czy scenariusz jest osiagany kiedykolwiek?
             * 
             * Odpowiedź 2:
             * Tak
             * 
             * Kwerenda 3:
             * Czy ¬a w chwili 4 zawsze?
             * 
             * Odpowiedź 3:
             * Nie
             * 
             * Kwerenda 4:
             * Czy ¬a w chwili 4 kiedykolwiek?
             * 
             * Odpowiedź 4:
             * Nie
             * 
             * Kwerenda 5:
             * Czy ¬l w chwili 4 zawsze?
             * 
             * Odpowiedź 5:
             * Nie
             * 
             * Kwerenda 6:
             * Czy ¬l w chwili 4 kiedykolwiek?
             * 
             * Odpowiedź 6:
             * Tak
             */

            #region Add specific formulas

            IFormula observationFormula1 = aFormula;

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 5) },
                ActionOccurrences = new List<ActionOccurrence> { }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery posibleScenarioQuery = new PossibleScenarioQuery(QueryType.Always, scenario.Id);
            IQuery posibleScenarioQuery2 = new PossibleScenarioQuery(QueryType.Ever, scenario.Id);
            IQuery formulaQuery = new FormulaQuery(4, negaFormula, scenario.Id, QueryType.Always);
            IQuery formulaQuery2 = new FormulaQuery(4, negaFormula, scenario.Id, QueryType.Ever);
            IQuery formulaQuery3 = new FormulaQuery(4, neglFormula, scenario.Id, QueryType.Always);
            IQuery formulaQuery4 = new FormulaQuery(4, neglFormula, scenario.Id, QueryType.Ever);

            #endregion

            #region Testing
            engine.SetMaxTime(5);

            bool responsePosibleScenarioQuery = engine.ExecuteQuery(posibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeTrue();
            bool responsePosibleScenarioQuery2 = engine.ExecuteQuery(posibleScenarioQuery2);
            responsePosibleScenarioQuery2.Should().BeTrue();
            bool responseFormulaQuery = engine.ExecuteQuery(formulaQuery);
            responseFormulaQuery.Should().BeFalse();
            bool responseFormulaQuery2 = engine.ExecuteQuery(formulaQuery2);
            responseFormulaQuery2.Should().BeFalse();
            bool responseFormulaQuery3 = engine.ExecuteQuery(formulaQuery3);
            responseFormulaQuery3.Should().BeFalse();
            bool responseFormulaQuery4 = engine.ExecuteQuery(formulaQuery4);
            responseFormulaQuery4.Should().BeTrue();

            #endregion
        }
    }
}
