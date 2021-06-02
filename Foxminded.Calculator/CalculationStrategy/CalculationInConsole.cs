using System;
using Foxminded.CalculationLibrary;
using Foxminded.CalculationLibrary.ShuntingYardParser;
using Foxminded.CalculatorApp.TextResourses;

namespace Foxminded.CalculatorApp.CalculationStrategy
{
    internal class CalculationInConsole : ICalculationStrategy
    {
        public void CalculateExpressions(Calculator calculator)
        {
            do
            {
                Console.WriteLine(Resources.IntroduceToEnterAnExpression);
                string expression = Console.ReadLine();
                if (expression == string.Empty) break;

                double result;
                try
                {
                    result = calculator.Calculate(expression);
                }
                catch (CalculationLibraryException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine(Resources.DevisionByZero);
                    continue;
                }

                Console.WriteLine(expression + @" = " + result);
            } while (true);
        }
    }
}
