using System;

namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    public class CalculationLibraryException : Exception
    {
        public DateTime ErrorTimeStamp { get; set; }
        public string CauseOfError { get; set; }

        public CalculationLibraryException()
        {
        }

        public CalculationLibraryException(string message)
            : base(message)
        {
        }

        public CalculationLibraryException(string message, string cause, DateTime time)
            : base(message)
        {
            CauseOfError = cause;
            ErrorTimeStamp = time;
        }
    }
}
