using System.Collections.Generic;

namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    public interface IParser
    {
        IEnumerable<string> GetTokensInReversePolishNotation(string expression);
    }
}
