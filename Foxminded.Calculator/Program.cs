using System;
using System.Globalization;
using System.Reflection;
using Foxminded.CalculationLibrary;
using Foxminded.CalculationLibrary.Reader;
using Foxminded.CalculationLibrary.ShuntingYardParser;
using Foxminded.CalculationLibrary.Writter;
using Foxminded.Calculator.TextResourses;

namespace Foxminded.Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Resources.Greeting, Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine(new string('=', 60));

            var inputOutput = new InputOutput();
            string filePath;
            try
            {
                filePath = inputOutput.GetFilePath(args);
            }
            catch (CalculatorException ex)
            {
                Console.WriteLine(ex.CauseOfError);
                return;
            }

            var computing = new Computing();

            if (filePath == string.Empty)
            {
                do
                {
                    Console.WriteLine(Resources.IntroduceToEnterAnExpression);
                    string expression = Console.ReadLine();
                    double result;
                    try
                    {
                        result = computing.Calculate(expression);
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
                } while (inputOutput.CalculateOneMoreExpresion());

                return;
            }

            IReader fileReader;
            try
            {
                fileReader = new FileReader(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] results = new string[fileReader.Data.Length];
            for (int i = 0; i < fileReader.Data.Length; i++)
            {
                try
                {
                    results[i] = computing.Calculate(fileReader.Data[i]).ToString(CultureInfo.InvariantCulture);
                }
                catch (CalculationLibraryException)
                {
                    results[i] = "expression error";
                }
                catch (DivideByZeroException)
                {
                    results[i] = "division by zero";
                }
            }

            string pathForResultsFile = inputOutput.GetResultsFilePath(filePath);
            try
            {
                IWritter writter = new FileWritter(pathForResultsFile);
                writter.WriteResultsIntoFile(fileReader.Data, results);
                Console.WriteLine(Resources.FileWithResults, pathForResultsFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
