using Foxminded.CalculationLibrary.ShuntingYardParser;

namespace Foxminded.CalculationLibrary.Tests
{
    internal class ComputingUnderTest : Computing
    {
        protected override IParser CreateParser()
        {
            return new StabParser();
        }
    }
}
