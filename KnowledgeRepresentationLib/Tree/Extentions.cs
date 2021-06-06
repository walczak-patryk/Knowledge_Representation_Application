using System.Collections.Generic;
using System.Linq;
using KR_Lib.DataStructures;
using KR_Lib.Structures;

namespace KR_Lib.Tree
{
    public static class ModelExtensions
    {
        public static List<IStructure> ToModels(this List<IStructure> structures)
        {
            var result = new List<IStructure>();
            foreach(var structure in structures)
            {
                result.Add(structure.ToModel());
            }
            return result;
        }
    }

    public static class StateExtensions
    {
        public static State Union(this State state1, State state2)
        {   
            var newState = state1.Clone() as State;
            // CurrentActions
            var currentActions1 = state1.CurrentActions.ToHashSet();
            var currentActions2 = state2.CurrentActions.ToHashSet();

            currentActions1.Union(currentActions2);
            newState.CurrentActions = currentActions1.ToList();

            // Fluents
            var fluents1 = state1.Fluents.ToHashSet();
            var fluents2 = state2.Fluents.ToHashSet();

            fluents1.Union(fluents2);
            newState.Fluents = fluents1.ToList();

            // ImpossibleActions
            var impossibleActions1 = state1.ImpossibleActions.ToHashSet();
            var impossibleActions2 = state2.ImpossibleActions.ToHashSet();

            impossibleActions1.Union(impossibleActions2);
            newState.ImpossibleActions = impossibleActions1.ToList();

            // FutureActions
            var futureActions1 = state1.FutureActions.ToHashSet();
            var futureActions2 = state2.FutureActions.ToHashSet();

            futureActions1.Union(futureActions2);
            newState.FutureActions = futureActions1.ToList();

            // InvalidDescription
            newState.InvalidDescription = state1.InvalidDescription || state2.InvalidDescription;

            return newState;
        }
    }
}

