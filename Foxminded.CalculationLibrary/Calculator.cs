using System;
using System.Collections.Generic;
using System.Globalization;
using Foxminded.CalculationLibrary.ShuntingYardParser;

namespace Foxminded.CalculationLibrary
{
    public class Calculator
    {
        private readonly NumberFormatInfo _numberFormat = new NumberFormatInfo {NumberDecimalSeparator = "."};

        public double Calculate(string expression)
        {
            if (expression == null) throw new ArgumentException();

            var parser = CreateParser(Convert.ToChar(_numberFormat.NumberDecimalSeparator));
            var rpnTokens = parser.GetTokensInReversePolishNotation(expression);

            var stack = new Stack<double>();

            foreach (string token in rpnTokens)
                if (double.TryParse(token, NumberStyles.Float, _numberFormat, out double tokNumber))
                    stack.Push(tokNumber);
                else
                    PerformAnArithmeticOperation(token, stack);

            return stack.Pop();
        }

        protected virtual IParser CreateParser(char decimalSeparator)
        {
            return new Parser(decimalSeparator);
        }

        private static void PerformAnArithmeticOperation(string token, Stack<double> stack)
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
}
