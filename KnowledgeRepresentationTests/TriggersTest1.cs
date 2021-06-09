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
    public class TriggersTest1
    {
        /// <summary>
        /// (a,1) casuses (not f)
        /// (not f) triggers (a,1)
        /// </summary>

        #region Variables

        IEngine engine;

        Action a;

        Fluent f;

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

            var fFormula = new Formula(f);
            negFFormula = new NegationFormula(fFormula);

            #endregion

            #region Add domain

            engine.AddStatement(new CauseStatement(new ActionTime(a, 1), negFFormula));
            engine.AddStatement(new TriggerStatement(new ActionTime(a, 1), negFFormula));
            
            #endregion
        }

        [TestMethod]
        public void TestScenario1()
        {
            /*
             *Obs={(not f, 0)}
             *Acs={}
             *
             *Kwerenda:
             *Czy akcja a jest wykonywana w dowolonym czasie?
             *
             *Odpowiedź:
             *Tak
             */

            #region Add scenarios

            IScenario scenario = new Scenario("testScenario1")
            {
                Observations = new List<Observation>() { new Observation(negFFormula, 0) },
                ActionOccurrences = new List<ActionOccurrence> { }
            };
            engine.AddScenario(scenario);

            #endregion

            #region Add querry

            IQuery query = new ActionQuery(3, a, scenario.Id);

            #endregion

            #region Testing

            engine.SetMaxTime(10);
            bool response = engine.ExecuteQuery(query);
            response.Should().BeTrue();

            #endregion
        }
    }
}
