using System;
using System.Collections.Generic;
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
}

