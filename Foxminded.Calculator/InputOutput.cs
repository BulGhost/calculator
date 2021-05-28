using System;
using System.IO;
using Foxminded.Calculator.TextResourses;

namespace Foxminded.Calculator
{
    class InputOutput
    {
        internal string GetFilePath(string[] args)
        {
            if (args != null && args.Length != 0 && FilePathInCommandLineIsCorrect(args))
            {
                Console.WriteLine(Resources.SpecifiedPath, args[0]);
                return args[0];
            }

            return InputPathToFile();
        }

        internal string GetResultsFilePath(string sourceFilePath)
        {
            if (sourceFilePath == null) throw new ArgumentNullException();

            FileInfo fileInfo = new FileInfo(sourceFilePath);
            string directory = fileInfo.DirectoryName;
            string fileName = fileInfo.Name.Insert(fileInfo.Name.LastIndexOf('.'), "_results");
            return directory + "\\" + fileName;
        }

        internal bool CalculateOneMoreExpresion()
        {
            Console.WriteLine(Resources.OfferToCalculateOneMoreExpresion);
            string answer = Console.ReadLine();

            Console.WriteLine(new string('-', 40));
            return answer?.ToUpper() == "Y";
        }

        private bool FilePathInCommandLineIsCorrect(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(Resources.InvalidFilePath);
                return false;
            }

            return File.Exists(args[0]);
        }

        private string InputPathToFile()
        {
            const int attemptsNum = 5;
            for (int i = 0; i < attemptsNum; i++)
            {
                Console.Write(Resources.IntriduceToEnterThePathToFile);
                string filePath = Console.ReadLine();

                if (filePath == string.Empty) return filePath;

                if (File.Exists(filePath)) return filePath;

                if (i < attemptsNum - 1)
                {
                    Console.WriteLine(Resources.InvalidFilePath +
                                      Resources.RemainingAttempts, attemptsNum - (i + 1));
                }
            }

            Console.WriteLine(Resources.AttemptsExhausted);
            CalculatorException exception =
                new CalculatorException(Resources.AttemptsExhausted, Resources.CauseOfError, DateTime.Now);
            throw exception;
        }
    }
}
