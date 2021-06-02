using Foxminded.CalculationLibrary.ShuntingYardParser;

namespace Foxminded.CalculationLibrary.Tests
{
    internal class CalculatorUnderTest : Calculator
    {
        protected override IParser CreateParser(char decimalSeparator)
        {
            return new StabParser();
        }
    }
}
