using Foxminded.CalculationLibrary.ShuntingYardParser;

namespace Foxminded.CalculationLibrary.Tests
{
    internal class ComputingUnderTest : Computing
    {
        protected override IParser CreateParser(char decimalSeparator)
        {
            return new StabParser();
        }
    }
}
