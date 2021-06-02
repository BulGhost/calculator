using System;
using System.IO;
using Foxminded.CalculatorApp.TextResourses;

namespace Foxminded.CalculatorApp
{
    internal class FilePathGetter
    {
        internal string GetFilePath(string[] args)
        {
            if (args != null && args.Length == 1 && File.Exists(args[0])) return args[0];

            Console.WriteLine(Resources.InvalidFilePath);
            Console.WriteLine(Resources.IntroduceToWorkInTheConsole);
            if (Console.ReadKey().Key != ConsoleKey.Y) throw new CalculatorAppException();

            Console.CursorLeft = 0;
            return null;
        }

        internal string GetResultsFilePath(string sourceFilePath)
        {
            if (sourceFilePath == null) throw new ArgumentNullException();

            FileInfo fileInfo = new FileInfo(sourceFilePath);
            string directory = fileInfo.DirectoryName;
            string fileName = fileInfo.Name.Insert(fileInfo.Name.LastIndexOf('.'),
                "_results_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));
            return directory + "\\" + fileName;
        }
    }
}
