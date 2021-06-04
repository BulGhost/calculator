using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Foxminded.CalculatorApp
{
    internal sealed class ProgramOptions
    {
        [Value(0, Required = false,
            HelpText = "The file path with expressions to calculate.", Default = "")]
        public string FilePath { get; set; }

        [Usage(ApplicationAlias = "Calculator.exe")]
        public static IEnumerable<Example> Examples =>
            new List<Example>
            {
                new Example("The application calculates arithmetic expressions and has two modes:" +
                            "\n1. Console mode - if there are no command line args." +
                            "\n2. Calculation of expressions from a file - if file path is specified in the command line",
                    new ProgramOptions {FilePath = @"D:\expr.txt"})
            };
    }
}
