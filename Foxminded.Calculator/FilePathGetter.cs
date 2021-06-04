using System;
using System.IO;

namespace Foxminded.CalculatorApp
{
    internal class FilePathGetter
    {
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
