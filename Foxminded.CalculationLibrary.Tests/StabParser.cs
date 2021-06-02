using System;
using System.Collections.Generic;
using Foxminded.CalculationLibrary.ShuntingYardParser;

namespace Foxminded.CalculationLibrary.Tests
{
    internal class StabParser : IParser
    {
        public ICollection<string> GetTokensInReversePolishNotation(string expression)
        {
            if (expression == "1") return new List<string> {"1", "2", "3", "2", "+", "*", "+"};

            if (expression == "2") return new List<string> {"2", "15", "3", "/", "+", "4", "2", "*", "+"};

            if (expression == "3") return new List<string> {"16.5", "32", "2", "3", "^", "/", "4", "*", "-", "-0.5", "+"};

            throw new ArgumentException();
        }
    }
}
