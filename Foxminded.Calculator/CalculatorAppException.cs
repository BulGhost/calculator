using System;

namespace Foxminded.CalculatorApp
{
    public class CalculatorAppException : Exception
    {
        public string CauseOfError { get; set; }

        public CalculatorAppException() { }

        public CalculatorAppException(string message, string cause)
            : base(message)
        {
            CauseOfError = cause;
        }
    }
}
