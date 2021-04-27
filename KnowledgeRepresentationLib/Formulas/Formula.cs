using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularFormulas;

using FormulaEvaluator;// <-- Nuget

// using KnowledgeRepresentationReasoning.Helpers;
using KR_Lib.DataStructures;

namespace KR_Lib.Formulas {



    public interface ILogicFormula
    {
        bool Evaluate(IEnumerable<Tuple<string, bool>> values);
        //       bool Evaluate(State state);
        void SetFormula(string Formula);
        string[] GetFluentNames();
        //List<Fluent[]> CalculatePossibleFluents();  // Nie ma grey'a
        void AddFormula(ILogicFormula logicFormula);
    }

  public class Formula : ILogicFormula {
    private readonly char[] _specialCharacters = new[] { '|', '&', '(', ')', '!' };

    private string _Formula;

    public Formula(Formula logicFormula) {
      this._Formula = logicFormula._Formula;
    }

    public Formula() {
      _Formula = string.Empty;
    }

    public Formula(string Formula) : this() {
      if (!string.IsNullOrEmpty(Formula)) {
        this._Formula = Formula;
      }
    }

    public bool Evaluate(IEnumerable<Tuple<string, bool>> values) {
      if (_Formula == null || _Formula.Equals(string.Empty)) {
        return true;
      }

      var Formula = new CompiledFormula(this._Formula);
      //Formula("h", typeof(FormulaHelper));
      //if (values != null) {
      //  foreach (var value in values) {
      //    Formula(value.Item1, value.Item2);
      //  }
      //}
      return (bool) Formula.Eval();
    }

    //public bool Evaluate(State state) {
    //  if (_Formula == null || _Formula.Equals(string.Empty)) {
    //    return true;
    //  }

    //  var Formula = new CompiledFormula(this._Formula);
    //  Formula.RegisterType("h", typeof(FormulaHelper));
    //  if (state != null) {
    //    foreach (var fluent in state.Fluents) {
    //      Formula.RegisterType(fluent.Name, fluent.Value);
    //    }
    //  }
    //  return (bool) Formula.Eval();
    //}

    public void SetFormula(string Formula) {
      this._Formula = Formula ?? string.Empty;
    }

    public string[] GetFluentNames() {
      string filteredString = this._Formula;
      filteredString = this._specialCharacters.Aggregate(filteredString, (current, specialCharacter) => current.Replace(specialCharacter, ' '));
      filteredString = Regex.Replace(filteredString, " {2,}", " ");
      return filteredString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
    }

    //public List<Fluent[]> CalculatePossibleFluents() {
    //  var result = new List<Fluent[]>();
    //  string[] fluentNames = this.GetFluentNames();
    //  int numberOfFluents = fluentNames.Length;
    //  foreach (var code in Gray.GetGreyCodesWithLengthN(numberOfFluents)) {
    //    var possibleFluents = new Fluent[numberOfFluents];
    //    for (int i = 0; i < numberOfFluents; i++) {
    //      possibleFluents[i] = new Fluent { Name = fluentNames[i], Value = code[i] };
    //    }
    //    if (this.Evaluate(possibleFluents.Select(t => new Tuple<string, bool>(t.Name, t.Value)))) {
    //      result.Add(possibleFluents);
    //    }
    //  }
    //  return result;
    //}

    private class FormulaHelper {
      public static bool impl(bool a, bool b) {
        if (a == false) {
          return true;
        }
        return b;
      }

      public static bool rown(bool a, bool b) {
        if (a == b) {
          return true;
        } else {
          return false;
        }
      }
    }

    public void AddFormula(ILogicFormula logicFormula) {
      if (logicFormula != null) {
        if (!string.IsNullOrEmpty(logicFormula.ToString()) && !string.IsNullOrEmpty(_Formula)) {
          _Formula = "(" + _Formula + ") && (" + logicFormula + ")";
        } else if (!string.IsNullOrEmpty(logicFormula.ToString())) {
          _Formula = logicFormula.ToString();
        }
      }
    }

    public override string ToString() {
      return this._Formula;
    }
  }
}
