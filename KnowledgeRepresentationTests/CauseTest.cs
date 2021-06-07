using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using KnowledgeRepresentationLib.Scenarios;
using KR_Lib;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using KR_Lib.Queries;
using KR_Lib.Scenarios;
using KR_Lib.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = KR_Lib.DataStructures.Action;

namespace KR_Tests
{
    [TestClass]
    public class CauseTest
    {
        /// <summary>
        /// (a, 1) causes f1
        /// (a, 1) causes f2
        /// (b, 1) causes f1
        /// (b, 1) causes -f1
        /// </summary>
        #region Variables

        IEngine engine;

        Action a;
        Action b;

        Fluent f1;
        Fluent f2;

        IFormula f1Formula;
        IFormula f2Formula;
        IFormula negf1Formula;
        IFormula negf2Formula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            a = new Action("a");
            engine.AddAction(a);
            b = new Action("b");
            engine.AddAction(b);

            #endregion

            #region Add fluents

            f1 = new Fluent("f1");
            engine.AddFluent(f1);

            f2 = new Fluent("f2");
            engine.AddFluent(f2);

            #endregion

            #region Add common formulas

            f1Formula = new Formula(f1);
            f2Formula = new Formula(f2);
            negf1Formula = new NegationFormula(f1Formula);
            negf2Formula = new NegationFormula(f2Formula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(a, 1), f1Formula));
            engine.AddStatement(new CauseStatement(new ActionTime(a, 1), f2Formula));
            engine.AddStatement(new CauseStatement(new ActionTime(b, 1), f1Formula));
            engine.AddStatement(new CauseStatement(new ActionTime(b, 1), negf1Formula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             *Obs={(-f1 ^ -f2, 0)}
             *Acs={(a,1,0)}
             *
             *Kwerenda:
             *Czy osiągalny jest stan f1∧f2 dla zadanego zbioru obserwacji?
             *
             *Odpowiedź:
             *Tak
             */
            #region Add specific formulas

            IFormula observationFormula1 = new ConjunctionFormula(negf1Formula, negf2Formula);

            #endregion

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(observationFormula1, 0) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(a, 1, 0) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new TargetQuery(new ConjunctionFormula(f1Formula, f2Formula), QueryType.Ever, scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(3);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             *Acs={(b,1,0)}
             *
             *Kwerenda:
             *Czy scenariusz jest osiagany kiedykolwiek?
             *
             *Odpowiedź:
             *Nie
            */

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario2")
            {
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(b, 1, 0) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery everPosibleScenarioQuery = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(3);
            bool responsePosibleScenarioQuery = engine.ExecuteQuery(everPosibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeFalse();

            #endregion
        }

    }
}
