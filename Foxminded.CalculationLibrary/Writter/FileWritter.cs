using System;
using System.IO;

namespace Foxminded.CalculationLibrary.Writter
{
    public class FileWritter : IWritter
    {
        private readonly string _path;

        public FileWritter(string pathForFile)
        {
            _path = pathForFile ?? throw new ArgumentNullException();
        }

        public void WriteResults(string[] expressionsToCalculate, string[] calculatedResults)
        {
            if (expressionsToCalculate == null || calculatedResults == null) throw new ArgumentNullException();

            File.WriteAllLines(_path, GetResultExpressions(expressionsToCalculate, calculatedResults));
        }

        private string[] GetResultExpressions(string[] expressionsToCalculate, string[] calculatedResults)
        {
            string[] calculatedExpressions = new string[expressionsToCalculate.Length];

            for (int i = 0; i < expressionsToCalculate.Length; i++)
                calculatedExpressions[i] = expressionsToCalculate[i] + " = " + calculatedResults[i];

            return calculatedExpressions;
        }
    }
}
