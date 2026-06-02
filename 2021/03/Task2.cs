using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var oxygenGeneratorRating = FilterDownToSingle(input, true);
        var co2scrubberRating = FilterDownToSingle(input, false);
        return Convert.ToInt32(oxygenGeneratorRating, 2) * Convert.ToInt32(co2scrubberRating, 2);
    }

    private string FilterDownToSingle(string[] numbers, bool keepHighest, int index = 0)
    {
        if (numbers.Length == 1) return numbers[0];
        IEnumerable<string> filteredNumbers;
        var onesMatch = numbers.Where(n => n[index] == '1');
        var zerosMatch = numbers.Where(n => n[index] == '0');
        if (onesMatch.Count() == zerosMatch.Count())
        {
            filteredNumbers = keepHighest ? onesMatch : zerosMatch;
        }

        filteredNumbers = (onesMatch.Count() < zerosMatch.Count()) == keepHighest ? onesMatch : zerosMatch;

        return FilterDownToSingle([.. filteredNumbers], keepHighest, index + 1);
    }
}