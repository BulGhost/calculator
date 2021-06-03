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
        private readonly char _decimalSeparator;

        private readonly IDictionary<string, Operator> _operators = new Dictionary<string, Operator>
        {
            ["+"] = new Operator { Name = "+", Precedence = 1 },
            ["-"] = new Operator { Name = "-", Precedence = 1 },
            ["*"] = new Operator { Name = "*", Precedence = 2 },
            ["/"] = new Operator { Name = "/", Precedence = 2 },
            ["^"] = new Operator { Name = "^", Precedence = 3, RightAssociative = true }
        };

        public Parser(char decimalSeparator)
        {
            _decimalSeparator = decimalSeparator;
        }

        public ICollection<string> GetTokensInReversePolishNotation(string expression)
        {
            if (expression == null) throw new ArgumentNullException();

            var tokensInInfixNotation = Tokenize(expression);

            var tokensInRpn = new List<string>();
            var stack = new Stack<string>();

            ProcessAllTokens(tokensInInfixNotation, tokensInRpn, stack);
            PopRemainingTokensFromStack(tokensInRpn, stack);

            return tokensInRpn;
        }

        internal IEnumerable<Token> Tokenize(string expression)
        {
            if (expression == null) throw new ArgumentNullException();

            RemoveExtraSpaces(ref expression);
            if (IsInvalidExpression(expression))
                throw new CalculationLibraryException("Invalid expression!");

            StringBuilder token = new StringBuilder();

            for (int i = 0; i < expression.Length; i++)
            {
                token.Append(expression[i]);

                if (IsCharAnUnaryMinus(expression, i)) continue;

                TokenType currType = DetermineType(expression[i]);

                if (currType == TokenType.Number && NextCharIsADigit(expression, i)) continue;

                yield return new Token(currType, token.ToString());

                token.Clear();
            }
        }

        private static void RemoveExtraSpaces(ref string expression)
        {
            expression = expression.Trim();
            if (expression[expression.Length - 1] == '=')
                expression = expression.Remove(expression.Length - 1, 1).TrimEnd();

            char[] charsAllowingSpacesBeside = { '+', '-', '*', '/', '(', ')' };
            for (int i = 1; i < expression.Length; i++)
                if (expression[i] == ' ' || expression[i] == '\t')
                {
                    int j = 1;
                    while (expression[i + j] == ' ' || expression[i + j] == '\t') j++;

                    if (charsAllowingSpacesBeside.Contains(expression[i - 1]) ||
                        charsAllowingSpacesBeside.Contains(expression[i + j]))
                        expression = expression.Remove(i, j);
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

        private TokenType DetermineType(char ch)
        {
            if (char.IsDigit(ch) || ch == _decimalSeparator)
                return TokenType.Number;
            if (ch == '(' || ch == ')')
                return TokenType.Parenthesis;
            if (_operators.ContainsKey(Convert.ToString(ch)))
                return TokenType.Operator;

            throw new Exception("Wrong character");
        }

        private bool NextCharIsADigit(string expr, int i)
        {
            return i + 1 < expr.Length && DetermineType(expr[i + 1]) == TokenType.Number;
        }

        private void ProcessAllTokens(IEnumerable<Token> tokens, List<string> rpnTokens, Stack<string> stack)
        {
            foreach (var tok in tokens)
                switch (tok.Type)
                {
                    case TokenType.Number:
                        rpnTokens.Add(tok.Value);
                        break;
                    case TokenType.Operator:
                        while (OperatorOnTopOfTheStackIsMorePriority(tok.Value, stack))
                            rpnTokens.Add(stack.Pop());

                        stack.Push(tok.Value);
                        break;
                    case TokenType.Parenthesis:
                        if (tok.Value == "(")
                        {
                            stack.Push(tok.Value);
                        }
                        else
                        {
                            while (stack.Peek() != "(") rpnTokens.Add(stack.Pop());
                            stack.Pop();
                        }

                        break;
                    default:
                        throw new Exception("Wrong token");
                }
        }

        private bool OperatorOnTopOfTheStackIsMorePriority(string tokValue, Stack<string> stack)
        {
            return stack.Any() && _operators.ContainsKey(stack.Peek()) && CompareOperators(tokValue, stack.Peek());
        }

        private bool CompareOperators(string op1, string op2) =>
            CompareOperators(_operators[op1], _operators[op2]);

        private static bool CompareOperators(Operator op1, Operator op2)
        {
            return op1.RightAssociative ? op1.Precedence < op2.Precedence : op1.Precedence <= op2.Precedence;
        }

        private static void PopRemainingTokensFromStack(ICollection<string> tokensInRpn, Stack<string> stack)
        {
            while (stack.Any())
            {
                string tok = stack.Pop();
                if (tok == "(" || tok == ")")
                    throw new Exception("Mismatched parentheses");

                tokensInRpn.Add(tok);
            }
        }
    }
}
