using System;
using Xunit;

namespace Foxminded.CalculationLibrary.Tests
{
    public class ComputingTests
    {
        [InlineData("1", 11.0)]
        [InlineData("2", 15.0)]
        [InlineData("3", 0.0)]
        [Theory]
        public void Calculate_ValidExpression_CalculatedResult(string expression, double expected)
        {
            double actual = new ComputingUnderTest().Calculate(expression);

            Assert.Equal(expected, actual, 3);
        }

        [Fact]
        public void Calculate_DivideByZero_DivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(() => new Computing().Calculate("2/0"));
        }
    }
}
