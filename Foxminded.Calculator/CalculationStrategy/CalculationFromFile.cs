using System;
using System.Globalization;
using Foxminded.CalculationLibrary;
using Foxminded.CalculationLibrary.Reader;
using Foxminded.CalculationLibrary.ShuntingYardParser;
using Foxminded.CalculationLibrary.Writter;
using Foxminded.CalculatorApp.TextResourses;

namespace Foxminded.CalculatorApp.CalculationStrategy
{
    internal class CalculationFromFile : ICalculationStrategy
    {
        private readonly string _filePath;

        public CalculationFromFile(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException();
        }

        public void CalculateExpressions(Calculator calculator)
        {
            IReader fileReader;
            try
            {
                fileReader = new FileReader(_filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            string[] results = GetResultsOfEachExpression(fileReader, calculator);

            string pathForResultsFile = new FilePathGetter().GetResultsFilePath(_filePath);
            WriteResultsInFile(pathForResultsFile, fileReader.Data, results);
        }

        private string[] GetResultsOfEachExpression(IReader fileReader, Calculator calculator)
        {
            var results = new string[fileReader.Data.Length];
            for (int i = 0; i < fileReader.Data.Length; i++)
                try
                {
                    results[i] = calculator.Calculate(fileReader.Data[i]).ToString(CultureInfo.InvariantCulture);
                }
                catch (CalculationLibraryException)
                {
                    results[i] = "expression error";
                }
                catch (DivideByZeroException)
                {
                    results[i] = "division by zero";
                }

            return results;
        }

        private static void WriteResultsInFile(string pathForResultsFile, string[] expressions, string[] results)
        {
            try
            {
                IWritter writter = new FileWritter(pathForResultsFile);
                writter.WriteResults(expressions, results);
                Console.WriteLine(Resources.FileWithResults, pathForResultsFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
