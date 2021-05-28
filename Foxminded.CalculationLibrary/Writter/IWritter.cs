namespace Foxminded.CalculationLibrary.Writter
{
    public interface IWritter
    {
        void WriteResultsIntoFile(string[] expressionsToCalculate, string[] calculatedResults);
    }
}
