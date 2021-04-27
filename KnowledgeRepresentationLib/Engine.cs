using KR_Lib.DataStructures;
using KR_Lib.Queries;
using KR_Lib.Scenarios;
using KR_Lib.Statements;

namespace KR_Lib
{
    interface IEngine
    {
        /// <summary>
        /// Adds fluent to list of fluents
        /// </summary>
        /// <param Fluent="fluent"></param>
        void AddFluent(Fluent fluent);

        /// <summary>
        /// Removes fluent from list of fluents
        /// </summary>
        /// <param Fluent="fluent"></param>
        void DeleteFluent(Fluent fluent);

        /// <summary>
        /// Add action to list of actions
        /// </summary>
        /// <param Action="action"></param>
        void AddAction(Action action);


        /// <summary>
        /// Removes action from list of actions
        /// </summary>
        /// <param Action="action"></param>
        void RemoveAction(Action action);

        /// <summary>
        /// Adds statement to list of statements
        /// </summary>
        /// <param IStatement="statement"></param>
        void AddStatement(IStatement statement);

        /// <summary>
        /// Removes statement from list of statements
        /// </summary>
        /// <param IStatement="statement"></param>
        void RemoveStatement(IStatement statement);

        /// <summary>
        /// Adds scenario to list of scenarios
        /// </summary>
        /// <param IScenario="scenario"></param>
        void AddScenario(IScenario scenario);

        /// <summary>
        /// Removes scenario from list of scenarios
        /// </summary>
        /// <param IScenario="scenario"></param>
        void RemoveScenario(IScenario scenario);

        /// <summary>
        /// Checks if querry is correct
        /// </summary>
        /// <param IQuery="querry"></param>
        /// <returns></returns>
        bool ExecuteQuerry(IQuery querry);

        // What else is needed? :
        // constructor for Action (string name, int timeDuration)
        // constructor for fluent (string name)
        // constructor for statement
        // constuctor for action occurance(Action action, int amoutOfOccurances)
        // constructor for observations(IlogicExpression logicExpression, int amoutOfObservations)
        // constructor for scenario (string scenarioName, ActionOccurance actionOccurance, List<IlogicExpression> logicExpressions) 
        // constructor for querry ([enum]? question type, [enum]? querry type)
    }
    class Engine : IEngine
    {
        public void AddAction(Action action)
        {
            throw new System.NotImplementedException();
        }

        public void AddFluent(Fluent fluent)
        {
            throw new System.NotImplementedException();
        }

        public void AddScenario(IScenario scenario)
        {
            throw new System.NotImplementedException();
        }

        public void AddStatement(IStatement statement)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFluent(Fluent fluent)
        {
            throw new System.NotImplementedException();
        }

        public bool ExecuteQuerry(IQuery querry)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAction(Action action)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveScenario(IScenario scenario)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveStatement(IStatement statement)
        {
            throw new System.NotImplementedException();
        }
    }
}
