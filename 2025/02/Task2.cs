using CommonLib.Solvers;
using System.Collections.Concurrent;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<(double start, double end)> sets = [.. input.First().Split(",").Select(set =>
        {
            var pair = set.Split("-");
            return (double.Parse(pair[0]), double.Parse(pair[1]));
        })];

        var invalidIds = new ConcurrentBag<double>();
        Parallel.ForEach(sets, set =>
        {
            for (double i = set.start; i <= set.end; i++)
            {
                var iString = i.ToString();
                var length = iString.Length;
                var isInvalid = Enumerable.Range(1, length).Any(j =>
                {
                    var testNumber = iString[..j];
                    var remainder = iString[j..].Chunk(j).Select(x => new string(x)).ToList();
                    return (remainder.Count != 0 && remainder.All(r => r == testNumber));
                });
                if (isInvalid)
                    invalidIds.Add(i);
            }
        });

        return invalidIds.Sum();
    }
}