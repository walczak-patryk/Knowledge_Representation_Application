using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ExpressionEvaluator;// <-- Nuget

// using KnowledgeRepresentationReasoning.Helpers;
using KR_Lib.DataStructures;

namespace KR_Lib.Expressions {



  public interface ILogicExpression {
    bool Evaluate(IEnumerable<Tuple<string, bool>> values);
    //       bool Evaluate(State state);
    void SetExpression(string expression);
    string[] GetFluentNames();
    //List<Fluent[]> CalculatePossibleFluents();  // Nie ma grey'a
    void AddExpression(ILogicExpression logicExpression);
  }


  public class LogicExpression : ILogicExpression {
    private readonly char[] _specialCharacters = new[] { '|', '&', '(', ')', '!' };

    private string _expression;

    public LogicExpression(LogicExpression logicExpression) {
      this._expression = logicExpression._expression;
    }

    public LogicExpression() {
      _expression = string.Empty;
    }

    public LogicExpression(string expression) : this() {
      if (!string.IsNullOrEmpty(expression)) {
        this._expression = expression;
      }
    }

    public bool Evaluate(IEnumerable<Tuple<string, bool>> values) {
      if (_expression == null || _expression.Equals(string.Empty)) {
        return true;
      }

      var expression = new CompiledExpression(this._expression);
      //expression("h", typeof(ExpressionHelper));
      //if (values != null) {
      //  foreach (var value in values) {
      //    expression(value.Item1, value.Item2);
      //  }
      //}
      return (bool) expression.Eval();
    }

    //public bool Evaluate(State state) {
    //  if (_expression == null || _expression.Equals(string.Empty)) {
    //    return true;
    //  }

    //  var expression = new CompiledExpression(this._expression);
    //  expression.RegisterType("h", typeof(ExpressionHelper));
    //  if (state != null) {
    //    foreach (var fluent in state.Fluents) {
    //      expression.RegisterType(fluent.Name, fluent.Value);
    //    }
    //  }
    //  return (bool) expression.Eval();
    //}

    public void SetExpression(string expression) {
      this._expression = expression ?? string.Empty;
    }

    public string[] GetFluentNames() {
      string filteredString = this._expression;
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

    private class ExpressionHelper {
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

    public void AddExpression(ILogicExpression logicExpression) {
      if (logicExpression != null) {
        if (!string.IsNullOrEmpty(logicExpression.ToString()) && !string.IsNullOrEmpty(_expression)) {
          _expression = "(" + _expression + ") && (" + logicExpression + ")";
        } else if (!string.IsNullOrEmpty(logicExpression.ToString())) {
          _expression = logicExpression.ToString();
        }
      }
    }

    public override string ToString() {
      return this._expression;
    }
  }
}
