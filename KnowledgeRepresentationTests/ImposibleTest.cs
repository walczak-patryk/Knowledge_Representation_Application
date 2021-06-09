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
    public class ImposibleTest
    {
        /// <summary>
        /// (a, 1) imposible if f
        /// (b, 1) causes -f
        /// </summary>
        #region Variables

        IEngine engine;

        Action a;
        Action b;

        Fluent f;


        IFormula fFormula;
        IFormula negfFormula;

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

            f = new Fluent("f");
            engine.AddFluent(f);

            #endregion

            #region Add common formulas

            fFormula = new Formula(f);
            negfFormula = new NegationFormula(fFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new ImpossibleIfStatement(new ActionTime(a, 1), fFormula));
            engine.AddStatement(new CauseStatement(new ActionTime(b, 1), negfFormula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             *Obs={(f, 0)}
             *Acs={(b,1,1)}
             *
             *Kwerenda:
             *Czy scenariusz jest osiągnalny?
             *
             *Odpowiedź:
             *Nie
             */

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(fFormula, 0) },
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(a, 1, 1) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(5);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeFalse();

            #endregion
        }

        [TestMethod]
        public void TestScenario2()
        {
            /*
             *Obs={(f, 0)}
             *Acs={(b,1,0), (a,1,1)}
             *
             *Kwerenda:
             *Czy scenariusz jest osiągnalny?
             *
             *Odpowiedź:
             *Tak
            */

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario2")
            {
                Observations = new List<Observation>() { new Observation(fFormula, 0)},
                ActionOccurrences = new List<ActionOccurrence> { new ActionOccurrence(b, 1, 0), new ActionOccurrence(a, 1, 1) }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery everPosibleScenarioQuery = new PossibleScenarioQuery(scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(5);
            bool responsePosibleScenarioQuery = engine.ExecuteQuery(everPosibleScenarioQuery);
            responsePosibleScenarioQuery.Should().BeTrue();

            #endregion
        }

    }
}
