using KR_Lib.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace KR_Lib.Formulas
{
    public static class FormulaParser
    {
        public static IFormula ParseToFormula(List<ObservationElement> observationElements)
        {
            if(observationElements == null)
            {
                return null;
            }
            Stack<ObservationElement> stack = new Stack<ObservationElement>();
            List<ObservationElement> observationElements_copy = observationElements.GetRange(0, observationElements.Count);
            foreach(var elem in observationElements_copy)
            {
                if(elem.isFluent)
                {
                    elem.formula = new Formula(elem.fluent);
                }
            }
            foreach (var elem in observationElements_copy)
            {
                if(elem.isFluent)
                {
                    stack.Push(elem);
                }
                else if(elem.operator_=="NOT")
                {
                    if (stack.Count < 1)
                    {
                        return null;
                    }
                    ObservationElement A = stack.Pop();
                    ObservationElement new_element = new ObservationElement(true, elem.fluent, elem.length, elem.operator_);
                    new_element.formula = new NegationFormula(A.formula);
                    stack.Push(new_element);
                }
                else
                {
                    if(stack.Count < 2)
                    {
                        return null;
                    }
                    ObservationElement A = stack.Pop();
                    ObservationElement B = stack.Pop();
                    ObservationElement new_element = new ObservationElement(true, elem.fluent, elem.length, elem.operator_);
                    switch (elem.operator_)
                    {
                        case "AND":
                            new_element.formula = new ConjunctionFormula(B.formula, A.formula);
                            stack.Push(new_element);
                            break;
                        case "OR":
                            new_element.formula = new AlternativeFormula(B.formula, A.formula);
                            stack.Push(new_element);
                            break;
                        case "=>":
                            new_element.formula = new ImplicationFormula(B.formula, A.formula);
                            stack.Push(new_element);
                            break;
                        case "<=>":
                            new_element.formula = new EquivalenceFormula(B.formula, A.formula);
                            stack.Push(new_element);
                            break;
                    }
                }
            }
            if(stack.Count > 1)
            {
                return null;
            }
            //show(stack.Peek().formula);
            return stack.Pop().formula;
        }

        static void show(IFormula formula)
        {
            HashSet<Fluent> list = formula.GetFluents();
            foreach(var elem in list)
            {
                Console.WriteLine(elem.Name);
            }
        }

        public static List<ObservationElement> infix_to_ONP(List<ObservationElement> observation)
        {
            if(observation.Count==0)
            {
                return null;
            }
            if(observation[observation.Count -1].operator_ == "NOT")
            {
                return null;
            }
            Dictionary<string, int> priorities = new Dictionary<string, int>();
            priorities.Add("AND", 2);
            priorities.Add("(", 0);
            priorities.Add(")", 1);
            priorities.Add("OR", 2);
            priorities.Add("NOT", 3);
            priorities.Add("=>", 1);
            priorities.Add("<=>", 1);
            List<ObservationElement> result = new List<ObservationElement>();

            Stack<ObservationElement> stack = new Stack<ObservationElement>();

            foreach (var elem in observation)
            {
                if (elem.isFluent)
                {
                    result.Add(elem);
                }
                else if (elem.operator_ == "(" || elem.operator_ == "NOT")
                {
                    stack.Push(elem);
                }
                else if (elem.operator_ == ")")
                {
                    while (stack.Count > 0)
                    {
                        if (stack.Peek().operator_ == "(")
                        {
                            stack.Pop();
                            break;
                        }
                        else
                        {
                            result.Add(stack.Pop());
                        }
                    }

                }
                else
                {
                    if (stack.Count == 0 || (priorities[elem.operator_] > priorities[stack.Peek().operator_]))
                    {
                        stack.Push(elem);
                    }
                    else
                    {
                        while (stack.Count > 0)
                        {
                            if (priorities[elem.operator_] <= priorities[stack.Peek().operator_])
                            {
                                result.Add(stack.Pop());
                            }
                            else
                            {
                                break;
                            }
                        }

                        stack.Push(elem);
                    }
                }

            }

            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }
            return result;
        }

        static public string print_obs(List<ObservationElement> observation)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var elem in observation)
            {
                if (elem.isFluent)
                {
                    sb.Append(elem.fluent.Name + " ");
                }
                else
                {
                    sb.Append(elem.operator_ + " ");
                }

            }
            return sb.ToString();
        }
    }
}