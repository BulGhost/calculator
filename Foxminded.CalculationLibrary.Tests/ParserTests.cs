using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Foxminded.CalculationLibrary.ShuntingYardParser;
using Xunit;

namespace Foxminded.CalculationLibrary.Tests
{
    public class ParserTests
    {
        private readonly Parser _parser = new Parser('.');

        public static IEnumerable<object[]> ExpressionsData =>
            new List<object[]>
            {
                new object[]
                {
                    "-2.2+(-4.8-22*-2)^31",
                    new List<string> {"-2.2", "-4.8", "22", "-2", "*", "-", "31", "^", "+"}
                },
                new object[]
                {
                    "5.6-(-2+3^3)/(-4.1)",
                    new List<string> {"5.6", "-2", "3", "3", "^", "+", "-4.1", "/", "-"}
                }
            };

        [InlineData("1-")]
        [InlineData("2+2)")]
        [InlineData("(2+2")]
        [InlineData("(1+(2+3)")]
        [InlineData("3+(4*)")]
        [InlineData("3+-(4*2)")]
        [InlineData("a+3")]
        [Theory]
        public void GetTokensInReversePolishNotation_InvalidExpression_Exception(string expression)
        {
            Assert.Throws<CalculationLibraryException>(() =>
                _parser.GetTokensInReversePolishNotation(expression).ToList());
        }

        [Theory]
        [MemberData(nameof(ExpressionsData))]
        public void GetTokensInReversePolishNotation_ValidExpression_TokensEnumeration
            (string expression, ICollection<string> rpnTokens)
        {
            var tokens = _parser.GetTokensInReversePolishNotation(expression);

            tokens.Should().Equal(rpnTokens);
        }
    }
}
