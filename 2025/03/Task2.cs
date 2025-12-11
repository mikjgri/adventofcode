using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var digitCount = 12;

        return input.Sum(line =>
        {
            var index = 0;
            string numberStr = "";
            for (var digitsLeft = digitCount; digitsLeft > 0; digitsLeft--)
            {
                var remainingValidOptions = Enumerable.Range(index, line.Length - index - digitsLeft + 1);
                var highestDigit = remainingValidOptions.Max(i => line[i]);
                numberStr += highestDigit.ToString();
                index = remainingValidOptions.First(i => line[i] == highestDigit) + 1;
            }
            return double.Parse(numberStr);
        });
    }
}