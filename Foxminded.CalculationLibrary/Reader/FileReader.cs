using System.IO;

namespace Foxminded.CalculationLibrary.Reader
{
    public class FileReader : IReader
    {
        public string[] Data { get; }

        public FileReader(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            Data = File.ReadAllLines(filePath);
        }
    }
}
