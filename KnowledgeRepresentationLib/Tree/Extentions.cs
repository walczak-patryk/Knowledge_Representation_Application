using System;
using System.Collections.Generic;
using KR_Lib.Models;

namespace KR_Lib.Tree
{
    public static class ModelExtensions
    {
        public static List<Structure> ToModels(this List<Structure> structures)
        {
            var result = new List<Structure>();
            foreach(var structure in structures)
            {
                result.Add(structure.ToModel());
            }
            return result;
        }
    }
}

