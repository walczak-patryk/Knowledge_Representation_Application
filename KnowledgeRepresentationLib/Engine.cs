using KR_Lib.DataStructures;
using KR_Lib.Queries;
using KR_Lib.Scenarios;
using KR_Lib.Statements;
using System;
using Action = System.Action;

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
        void RemoveFluent(Guid id);

        /// <summary>
        /// Add action to list of actions
        /// </summary>
        /// <param Action="action"></param>
        void AddAction(Action action);

        /// <summary>
        /// Removes action from list of actions
        /// </summary>
        /// <param Action="action"></param>
        void RemoveAction(Guid id);

        /// <summary>
        /// Adds statement to list of statements
        /// </summary>
        /// <param IStatement="statement"></param>
        void AddStatement(IStatement statement);

        /// <summary>
        /// Removes statement from list of statements
        /// </summary>
        /// <param IStatement="statement"></param>
        void RemoveStatement(Guid id);

        /// <summary>
        /// Adds scenario to list of scenarios
        /// </summary>
        /// <param IScenario="scenario"></param>
        void AddScenario(IScenario scenario);

        /// <summary>
        /// Removes scenario from list of scenarios
        /// </summary>
        /// <param name="id"></param>
        void RemoveScenario(Guid id);

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
            throw new NotImplementedException(); 
        }

        public void AddFluent(Fluent fluent)
        {
            throw new NotImplementedException();
        }

        public void AddScenario(IScenario scenario)
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement statement)
        {
            throw new NotImplementedException();
        }

        public bool ExecuteQuerry(IQuery querry)
        {
            /*
            stworzenie drzewa możliwości zacznjącego sie od stanu pierwotnego
            generującego wszystkie możliwe scieżki/stany w węzłach

            przetworzenie tego drzewa na modele


            if ( sprawdzenie czy kwerenda odnosi sie do akcji czy formuly ) {
            # zapytanie o formule #
                 foreach( var modelu in modelach){
                    if ( model.H(a,t) != 1)
                        return false;
                }
             return true;
             if ( model.H(a,t) 
                foreach( var modelu in mofelach){
                    if ( !((A,t) in model.E))
                        return false;
                }
             return true;
            }
            else{
            # zapytanie o akcje #

            }
            */
            return false;
        }

        public void RemoveAction(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveFluent(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveScenario(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveStatement(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
