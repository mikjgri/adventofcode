using CommonLib;
using System.Collections.Concurrent;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var digitCount = 12;

        var highestNumbers = new ConcurrentBag<double>();
        Parallel.ForEach(input, line =>
        {
            var lineLength = line.Length;
            var highestAtEachSize = new Dictionary<int, double>();
            var allowedRange = Enumerable.Range(0, lineLength - digitCount+1);
            var highestStartDigit = allowedRange.Max(i => line[i]);
            var bestStarts = allowedRange.Where(i => line[i] == highestStartDigit).ToList();

            var highest = bestStarts.Max(x => GetHighest(x, 0, digitCount));
            highestNumbers.Add(highest);
            double GetHighest(int index, double number, int digitsLeft)
            {
                if (digitsLeft == 0) return number;
                if (index >= lineLength) return 0;

                var currentSize = digitCount - digitsLeft;
                if (!highestAtEachSize.TryGetValue(currentSize, out var currentHighest) || currentHighest < number) highestAtEachSize[currentSize] = number;
                if (currentHighest > number) return number; //seen better path. escape early

                number = double.Parse(number.ToString() + line[index]);

                double bestSubPath = 0;
                for (var i = index + 1; i <= lineLength; i++)
                {
                    var pathResult = GetHighest(i, number, digitsLeft - 1);
                    if (pathResult > bestSubPath) bestSubPath = pathResult;
                }
                return bestSubPath;
            }
        });
        Console.WriteLine(highestNumbers.Sum());
    }
}