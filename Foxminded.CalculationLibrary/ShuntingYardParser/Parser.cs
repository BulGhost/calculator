using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Foxminded.CalculationLibrary.Tests")]

namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    internal class Parser : IParser
    {
        private const char DecimalSeparator = '.';

        private readonly IDictionary<string, Operator> _operators = new Dictionary<string, Operator>
        {
            ["+"] = new Operator {Name = "+", Precedence = 1},
            ["-"] = new Operator {Name = "-", Precedence = 1},
            ["*"] = new Operator {Name = "*", Precedence = 2},
            ["/"] = new Operator {Name = "/", Precedence = 2},
            ["^"] = new Operator {Name = "^", Precedence = 3, RightAssociative = true}
        };

        public IEnumerable<string> GetTokensInReversePolishNotation(string expression)
        {
            if (expression == null) throw new ArgumentNullException();

            var tokens = Tokenize(expression);

            var stack = new Stack<string>();
            foreach (var tok in tokens)
            {
                switch (tok.Type)
                {
                    case TokenType.Number:
                        yield return tok.Value;

                        break;
                    case TokenType.Operator:
                        while (stack.Any() && IsOperator(stack.Peek()) &&
                               CompareOperators(tok.Value, stack.Peek()))
                            yield return stack.Pop();

                        stack.Push(tok.Value);
                        break;
                    case TokenType.Parenthesis:
                        if (tok.Value == "(")
                            stack.Push(tok.Value);
                        else
                        {
                            while (stack.Peek() != "(")
                                yield return stack.Pop();

                            stack.Pop();
                        }

                        break;
                    default:
                        throw new Exception("Wrong token");
                }
            }

            while (stack.Any())
            {
                string tok = stack.Pop();
                if (tok == "(" || tok == ")")
                    throw new Exception("Mismatched parentheses");

                yield return tok;
            }
        }

        internal IEnumerable<Token> Tokenize(string expr)
        {
            if (expr == null) throw new ArgumentNullException();

            if (IsInvalidExpression(expr)) throw new CalculationLibraryException("Invalid expression!");

            StringBuilder token = new StringBuilder();

            for (int i = 0; i < expr.Length; i++)
            {
                token.Append(expr[i]);

                if (IsCharAnUnaryMinus(expr, i)) continue;

                TokenType currType = DetermineType(expr[i]);

                if (currType == TokenType.Number && NextCharIsADigit(expr, i)) continue;

                yield return new Token(currType, token.ToString());

                token.Clear();
            }
        }

        private static bool IsInvalidExpression(string expression)
        {
            var regex = new Regex(@"(?x)
                ^
                (?> (?<p> \( )* (?>-?\d+(?:\.\d+)?) (?<-p> \) )* )
                (?>(?:
                    [-+*/^]
                    (?> (?<p> \( )* (?>-?\d+(?:\.\d+)?) (?<-p> \) )* )
                )*)
                (?(p)(?!))
                $
            ");

            return !regex.IsMatch(expression);
        }

        private bool IsCharAnUnaryMinus(string expr, int i)
        {
            return expr[i] == '-' &&
                   (i == 0 || expr[i - 1] == '(' || DetermineType(expr[i - 1]) == TokenType.Operator) &&
                   DetermineType(expr[i + 1]) == TokenType.Number;
        }

        private bool NextCharIsADigit(string expr, int i)
        {
            return i + 1 < expr.Length && DetermineType(expr[i + 1]) == TokenType.Number;
        }

        private bool CompareOperators(string op1, string op2) =>
            CompareOperators(_operators[op1], _operators[op2]);

        private static bool CompareOperators(Operator op1, Operator op2)
        {
            return op1.RightAssociative ? op1.Precedence < op2.Precedence : op1.Precedence <= op2.Precedence;
        }

        private TokenType DetermineType(char ch)
        {
            if (char.IsDigit(ch) || ch == DecimalSeparator)
                return TokenType.Number;
            if (ch == '(' || ch == ')')
                return TokenType.Parenthesis;
            if (_operators.ContainsKey(Convert.ToString(ch)))
                return TokenType.Operator;

            throw new Exception("Wrong character");
        }

        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" ||
                   token == "*" || token == "/" || token == "^";
        }
    }
}
