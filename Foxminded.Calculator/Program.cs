using System;
using System.Reflection;
using Foxminded.CalculationLibrary;
using Foxminded.CalculatorApp.CalculationStrategy;
using Foxminded.CalculatorApp.TextResourses;

namespace Foxminded.CalculatorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Resources.Greeting, Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine(new string('=', 60));

            string filePath = new FilePathGetter().GetFilePath(args);

            var calculationStrategy = filePath == null
                ? (ICalculationStrategy) new CalculationInConsole()
                : new CalculationFromFile(filePath);
            var context = new Context(calculationStrategy);

            context.RunCalculator(new Calculator());
        }
    }
}
