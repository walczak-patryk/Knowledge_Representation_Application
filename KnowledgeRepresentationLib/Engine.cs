using KR_Lib.DataStructures;
using KR_Lib.Descriptions;
using KR_Lib.Exceptions;
using KR_Lib.Structures;
using KR_Lib.Queries;
using KR_Lib.Scenarios;
using KR_Lib.Statements;
using KR_Lib.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = KR_Lib.DataStructures.Action;
using KR_Lib.Formulas;

namespace KR_Lib
{
    public interface IEngine
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
        /// Adds scenario to list of scenarios
        /// </summary>
        /// <param IScenario="scenario"></param>
        void AddObservation(Guid scenarioId, List<ObservationElement> observationElements, int time);

        /// <summary>
        /// Checks if query is correct
        /// </summary>
        /// <param IQuery="query"></param>
        /// <returns></returns>
        bool ExecuteQuery(IQuery query);

        /// <summary>
        /// Set maximal time of computation.
        /// </summary>
        /// <param name="time"></param>
        void SetMaxTime(int time);

        // What else is needed? :
        // constructor for Action (string name, int timeDuration)
        // constructor for fluent (string name)
        // constructor for statement
        // constuctor for action occurance(Action action, int amoutOfOccurances)
        // constructor for observations(IlogicExpression logicExpression, int amoutOfObservations)
        // constructor for scenario (string scenarioName, ActionOccurance actionOccurance, List<IlogicExpression> logicExpressions) 
        // constructor for query ([enum]? question type, [enum]? query type)
    }
    public class Engine : IEngine
    {
        private IDescription description = new Description();
        private List<IScenario> scenarios = new List<IScenario>();
        private List<Action> actions = new List<Action>();
        private List<Fluent> fluents = new List<Fluent>();
        private List<IStructure> modeledStructures;
        private bool newChangesFlag = true;
        private Guid currentScenarioId;
        private int maxTime;

        private void GenerateModels(IScenario scenario) 
        {
            var root = TreeMethods.GenerateTree(description, scenario, maxTime);
            var structures = TreeMethods.GenerateStructues(root, scenario);
            this.modeledStructures = structures.ToModels();
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
            scenarios.Add(scenario);
        }

        /// <summary>
        /// Adds scenario to list of scenarios
        /// </summary>
        /// <param IScenario="scenario"></param>
        public void AddObservation(Guid scenarioId, List<ObservationElement> observationElements, int time) 
        {
            newChangesFlag = true;
            var observation = FormulaParser.ParseToFormula(observationElements);
            var scenario = scenarios.Where(s => s.Id == scenarioId).FirstOrDefault();
            if (scenario != null)
                scenario.AddObservation(new Observation(observation, time));
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
            var scenarioToRemove = scenarios.SingleOrDefault(scenario => scenario.Id == id);
            if (scenarioToRemove != null)
                scenarios.Remove(scenarioToRemove);
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
        /// Set maximal time of computation.
        /// </summary>
        /// <param name="time"></param>
        public void SetMaxTime(int time)
        {
            newChangesFlag = true;
            this.maxTime = time;
        }

        /// <summary>
        /// Checks if query is correct
        /// </summary>
        /// <param IQuery="query"></param>
        /// <returns></returns>
        public bool ExecuteQuery(IQuery query)
        {
            var selectedScenario = scenarios.SingleOrDefault(scenario => scenario.Id == query.ScenarioId);
            if (selectedScenario == null)
                throw new ScenarioNoExistsException("Scenariusz nie istnieje");


            if (newChangesFlag || currentScenarioId != selectedScenario.Id )
            {
                GenerateModels(selectedScenario);
                currentScenarioId = selectedScenario.Id;
                newChangesFlag = false;
            }

            return query.GetAnswer(modeledStructures);           
        }
    }
}
