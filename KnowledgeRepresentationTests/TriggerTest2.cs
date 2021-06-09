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
    public class TriggersTest2
    {
        /// <summary>
        /// (a,1) casuses (f or not f)
        /// (f) triggers (a,1)
        /// </summary>

        #region Variables

        IEngine engine;

        Action a;

        Fluent f;

        IFormula fFormula;
        IFormula negFFormula;

        #endregion

        [TestInitialize()]
        public void MyTestInitialize()
        {
            engine = new Engine();

            #region Add actions

            a = new Action("a");
            engine.AddAction(a);

            #endregion

            #region Add fluents

            f = new Fluent("f");
            engine.AddFluent(f);

            #endregion

            #region Add common formulas

            fFormula = new Formula(f);
            negFFormula = new NegationFormula(fFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(a, 1), new AlternativeFormula(fFormula, negFFormula)));
            engine.AddStatement(new TriggerStatement(new ActionTime(a, 1), fFormula));

            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             *Obs={(f, 0)}
             *Acs={}
             *
             *Kwerenda:
             *Czy akcja a jest wykonywana w dowolonym czasie > 1?
             *
             *Odpowiedź:
             *Nie
             */

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(fFormula, 0) },
                ActionOccurrences = new List<ActionOccurrence> { }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(0, a, scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(10);

            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            query = new ActionQuery(1, a, scenario.Id);
            response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            query = new ActionQuery(2, a, scenario.Id);
            response = engine.ExecuteQuery(query);
            response.Should().BeFalse();

            query = new ActionQuery(3, a, scenario.Id);
            response = engine.ExecuteQuery(query);
            response.Should().BeFalse();

            query = new ActionQuery(4, a, scenario.Id);
            response = engine.ExecuteQuery(query);
            response.Should().BeFalse();

            #endregion
        }
    }
}
