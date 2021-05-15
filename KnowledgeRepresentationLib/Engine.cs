using KR_Lib.DataStructures;
using KR_Lib.Descriptions;
using KR_Lib.Exceptions;
using KR_Lib.Models;
using KR_Lib.Queries;
using KR_Lib.Scenarios;
using KR_Lib.Statements;
using KR_Lib.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = KR_Lib.DataStructures.Action;

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
        IDescription description = new Description();
        IScenario scenario = new Scenario();
        List<Action> actions = new List<Action>();
        List<Fluent> fluents = new List<Fluent>();
        List<Structure> models;
        private bool newChangesFlag = true;

        private void GenerateModels() 
        {
            var root = TreeMethods.GenerateTree(description, scenario); //Kacper, Kacper, Kornel
            var structures = TreeMethods.GenerateStructues(root); //Kacper, Kacper, Kornel
            this.models = structures.ToModels(); //Ala, Filip
        }

        /// <summary>
        /// Add action to list of actions
        /// </summary>
        /// <param Action="action"></param>
        public void AddAction(Action action)
        {
            newChangesFlag = true;
            actions.Add(action);
        }

        /// <summary>
        /// Adds fluent to list of fluents
        /// </summary>
        /// <param Fluent="fluent"></param>
        public void AddFluent(Fluent fluent)
        {
            newChangesFlag = true;
            fluents.Add(fluent);
        }

        /// <summary>
        /// Adds scenario to list of scenarios
        /// </summary>
        /// <param IScenario="scenario"></param>
        public void AddScenario(IScenario scenario)
        {
            newChangesFlag = true;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds statement to list of statements
        /// </summary>
        /// <param IStatement="statement"></param>
        public void AddStatement(IStatement statement)
        {
            newChangesFlag = true;
            this.description.AddStatement(statement);
        }

        /// <summary>
        /// Removes action from list of actions
        /// </summary>
        /// <param Action="action"></param>
        public void RemoveAction(Guid id)
        {
            newChangesFlag = true;
            var actionToRemove = actions.SingleOrDefault(action => action.Id == id);
            if (actionToRemove != null)
                actions.Remove(actionToRemove);
        }


        /// <summary>
        /// Removes fluent from list of fluents
        /// </summary>
        /// <param Fluent="fluent"></param>
        public void RemoveFluent(Guid id)
        {
            newChangesFlag = true;
            var fluentToRemove = fluents.SingleOrDefault(fluent => fluent.Id == id);
            if (fluentToRemove != null)
                fluents.Remove(fluentToRemove);
        }

        /// <summary>
        /// Removes scenario from list of scenarios
        /// </summary>
        /// <param name="id"></param>
        public void RemoveScenario(Guid id)
        {
            newChangesFlag = true;
            var fluentToRemove = fluents.SingleOrDefault(fluent => fluent.Id == id);
            if (fluentToRemove != null)
                fluents.Remove(fluentToRemove);
        }

        /// <summary>
        /// Removes statement from list of statements
        /// </summary>
        /// <param IStatement="statement"></param>
        public void RemoveStatement(Guid id)
        {
            newChangesFlag = true;
            this.description.DeleteStatement(id);
        }

        /// <summary>
        /// Checks if querry is correct
        /// </summary>
        /// <param IQuery="querry"></param>
        /// <returns></returns>
        public bool ExecuteQuerry(IQuery querry)
        {
            if (newChangesFlag)
            {
                GenerateModels();
                newChangesFlag = false;
            }

            if (models.Count == 0)
                throw new InconsistentException();

            //Ala, Filip
            // 1.1. Czy podany scenariusz jest możliwy do realizacji kiedykolwiek?
            // 1.2. Czy podany scenariusz jest możliwy do realizacji zawsze?
            // 2. Czy w chwili t realizacji scenariusza wykonywana jest akcja A?
            // 3.1. Czy w chwili t ≥ 0 realizacji podanego scenariusza warunek γ zachodzi kiedykolwiek?
            // 3.2. Czy w chwili t ≥ 0 realizacji podanego scenariusza warunek γ zachodzi zawsze?
            // 4.1. Czy podany cel γ jest osiągalny kiedykolwiek przy zadanym zbiorze obserwacji OBS?
            // 4.2. Czy podany cel γ jest osiągalny zawsze przy zadanym zbiorze obserwacji OBS?
            if (querry is FormulaQuery)
            {
                var q = querry as FormulaQuery;
                foreach (var model in this.models)
                {
                    if (model.H(q.Formula, q.Time) != 1)
                        return false;
                }
            }
            else
            {
                var q = querry as FluentQuery;
                foreach (var model in this.models)
                {
                    if (!model.E.Contains((q.Action, q.Time)))
                        return false;
                }
            }
            return true;
        }
    }
}
