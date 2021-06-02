using System;

namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    public class CalculationLibraryException : Exception
    {
        public string CauseOfError { get; set; }

        public CalculationLibraryException()
        {
        }

        public CalculationLibraryException(string message)
            : base(message)
        {
        }

        public CalculationLibraryException(string message, string cause)
            : base(message)
        {
            CauseOfError = cause;
        }
    }
}
