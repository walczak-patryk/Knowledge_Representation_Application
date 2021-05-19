using KR_Lib;
using KR_Lib.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;
using Action = KR_Lib.DataStructures.Action;
using KR_Lib.Descriptions;
using KR_Lib.Statements;

namespace KR_Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FirstExampleTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            IEngine engine = new Engine();

            #region Add fluents and actions

            var hardWorking = new Action("hard_working", 8, 0);
            var shopping = new Action("shopping", 2, 8);

            #endregion

            #region Add domain

            IDescription description = new Description();

            description.AddStatement(new CauseStatement());

            #endregion

            #region Add scenarios
            #endregion

            #region Add querry
            IQuery query = new ActionQuery(1, new Action(), Guid.NewGuid());
            #endregion

            var response = engine.ExecuteQuery(query);
            response.Should().BeTrue();            
        }
    }
}
