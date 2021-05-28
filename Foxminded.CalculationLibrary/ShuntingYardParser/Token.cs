using System;

namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    internal enum TokenType
    {
        Number,
        Parenthesis,
        Operator
    }

    internal readonly struct Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}