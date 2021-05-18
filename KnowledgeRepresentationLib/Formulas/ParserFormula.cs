//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KR_Lib.Formulas
//{
//    public static class ParserFormula
//    {
//        public List<ObservationElement> infix_to_ONP(List<ObservationElement> observation)
//        {
//            Dictionary<string, int> priorities = new Dictionary<string, int>();
//            priorities.Add("AND", 2);
//            priorities.Add("(", 0);
//            priorities.Add(")", 1);
//            priorities.Add("OR", 2);
//            priorities.Add("NOT", 3);
//            priorities.Add("=>", 1);
//            priorities.Add("<=>", 1);
//            List<ObservationElement> result = new List<ObservationElement>();

//            Stack<ObservationElement> stack = new Stack<ObservationElement>();

//            foreach (var elem in observation)
//            {
//                if (elem.isFluent)
//                {
//                    result.Add(elem);
//                }
//                else if (elem.operator_ == "(")
//                {
//                    stack.Push(elem);
//                }
//                else if (elem.operator_ == ")")
//                {
//                    while (stack.Count > 0)
//                    {
//                        if (stack.Peek().operator_ == "(")
//                        {
//                            stack.Pop();
//                            break;
//                        }
//                        else
//                        {
//                            result.Add(stack.Pop());
//                        }
//                    }

//                }
//                else
//                {
//                    if (stack.Count == 0 || (priorities[elem.operator_] > priorities[stack.Peek().operator_]))
//                    {
//                        stack.Push(elem);
//                    }
//                    else
//                    {
//                        while (stack.Count > 0)
//                        {
//                            if (priorities[elem.operator_] <= priorities[stack.Peek().operator_])
//                            {
//                                result.Add(stack.Pop());
//                            }
//                            else
//                            {
//                                break;
//                            }
//                        }

//                        stack.Push(elem);
//                    }
//                }

//            }

//            while (stack.Count > 0)
//            {
//                result.Add(stack.Pop());
//            }
//            print_obs(result);
//            return result;
//        }
//    }
//}
