using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var nodes = input.Select(line =>
        {
            var split = line.Split(": ");
            return (split[0], split[1].Split(" ").Select(str => str).ToArray());
        }).ToDictionary(a => a.Item1, a => a.Item2);
        nodes.Add("out", []);

        var memoizationCache = new Dictionary<string, long>();

        var result = RunToOut("svr", false, false);
        return result;

        long RunToOut(string nodeId, bool visitedDac, bool visitedFft)
        {
            if (nodeId == "out" && visitedDac && visitedFft) return 1;
            if (nodeId == "dac") visitedDac = true;
            if (nodeId == "fft") visitedFft = true;

            var cacheKey = $"{nodeId}{visitedDac}{visitedFft}";
            if (memoizationCache.TryGetValue(cacheKey, out var cacheVal))
            {
                return cacheVal;
            }

            long sum = 0;
            foreach (var child in nodes[nodeId])
            {
                sum += RunToOut(child, visitedDac, visitedFft);
            }
            memoizationCache.Add(cacheKey, sum);
            return sum;
        }
    }
}