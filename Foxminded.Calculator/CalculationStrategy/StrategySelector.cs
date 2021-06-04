using System;
using System.IO;
using System.Reflection;
using Foxminded.CalculatorApp.TextResourses;

namespace Foxminded.CalculatorApp.CalculationStrategy
{
    internal class StrategySelector
    {
        public void ChooseCalculationStrategy(ProgramOptions progOptions, out ICalculationStrategy calculationStrategy)
        {
            calculationStrategy = null;
            if (progOptions.FilePath == string.Empty)
            {
                Console.WriteLine(Resources.Greeting, Assembly.GetExecutingAssembly().GetName().Version);
                Console.WriteLine(new string('=', 60));
                calculationStrategy = new CalculationInConsole();
            }
            else
            {
                try
                {
                    calculationStrategy = new CalculationFromFile(progOptions.FilePath);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine(Resources.InvalidFilePath);
                }
            }
        }
    }
}
