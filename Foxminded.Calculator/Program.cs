using System;
using Foxminded.CalculationLibrary;
using Foxminded.CalculatorApp.CalculationStrategy;
using CommandLine;

namespace Foxminded.CalculatorApp
{
    class Program
    {
        static int Main(string[] args)
        {
            var progOptions = new ProgramOptions();
            var result = Parser.Default.ParseArguments<ProgramOptions>(args)
                .WithParsed(options => progOptions = options);
            if (result.Tag != ParserResultType.Parsed) return -1;

            new StrategySelector().ChooseCalculationStrategy(progOptions, out var calculationStrategy);
            if (calculationStrategy == null) return -1;

            var context = new Context(calculationStrategy);
            context.RunCalculator(new Calculator());

            return 0;
        }
    }
}
