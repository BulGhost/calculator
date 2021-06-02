using System;
using Foxminded.CalculationLibrary;

namespace Foxminded.CalculatorApp.CalculationStrategy
{
    internal class Context
    {
        private readonly ICalculationStrategy _calculationStrategy;

        public Context(ICalculationStrategy calculationStrategy)
        {
            _calculationStrategy = calculationStrategy ?? throw new ArgumentNullException();
        }

        public void RunCalculator(Calculator calculator)
        {
            if (calculator == null) throw new ArgumentNullException();

            _calculationStrategy.CalculateExpressions(calculator);
        }
    }
}
