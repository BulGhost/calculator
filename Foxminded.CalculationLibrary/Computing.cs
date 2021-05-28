using System;
using System.Collections.Generic;
using System.Globalization;
using Foxminded.CalculationLibrary.ShuntingYardParser;

namespace Foxminded.CalculationLibrary
{
    public class Computing
    {
        private readonly NumberFormatInfo _numberFormat = new NumberFormatInfo {NumberDecimalSeparator = "."};

        public double Calculate(string expression)
        {
            if (expression == null) throw new ArgumentException();

            var parser = CreateParser();
            var rpnTokens = parser.GetTokensInReversePolishNotation(expression);

            Stack<double> stack = new Stack<double>();

            foreach (var token in rpnTokens)
            {
                if (double.TryParse(token, NumberStyles.Float, _numberFormat, out double tokNumber))
                {
                    stack.Push(tokNumber);
                }
                else
                {
                    double number;
                    if (token == "^")
                    {
                        number = stack.Pop();
                        stack.Push(Math.Pow(stack.Pop(), number));
                    }
                    else if (token == "*")
                    {
                        stack.Push(stack.Pop() * stack.Pop());
                    }
                    else if (token == "/")
                    {
                        number = stack.Pop();
                        if (number == 0) throw new DivideByZeroException();

                        stack.Push(stack.Pop() / number);
                    }
                    else if (token == "+")
                    {
                        stack.Push(stack.Pop() + stack.Pop());
                    }
                    else if (token == "-")
                    {
                        number = stack.Pop();
                        stack.Push(stack.Pop() - number);
                    }
                    else
                    {
                        throw new Exception("Wrong token");
                    }
                }
            }

            return stack.Pop();
        }

        protected virtual IParser CreateParser()
        {
            return new Parser();
        }
    }
}
