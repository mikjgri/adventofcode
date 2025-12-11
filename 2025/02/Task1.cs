using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<(double start, double end)> sets = [.. input.First().Split(",").Select(set =>
        {
            var pair = set.Split("-");
            return (double.Parse(pair[0]), double.Parse(pair[1]));
        })];

        return sets.Sum(set =>
            set.start.Range(set.end - set.start + 1).Sum(i =>
            {
                var iString = i.ToString();
                var halfLength = iString.Length / 2;

                if (double.IsOddInteger(iString.Length)) 
                    return 0;
                if (iString[..halfLength] == iString[halfLength..])
                    return i;
                return 0;
            })
        );
    }
}