using System.Collections.Generic;

namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    public interface IParser
    {
        ICollection<string> GetTokensInReversePolishNotation(string expression);
    }
}
