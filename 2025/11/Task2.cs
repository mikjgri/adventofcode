using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        //build node structure
        var childrenTempDict = new Dictionary<string, List<string>>();
        var nodes = input.Select(line =>
        {
            var split = line.Split(": ");
            childrenTempDict.Add(split[0], [.. split[1].Split(" ").Select(str => str)]);
            return new Node(split[0]);
        }).ToList();
        var endNode = new Node("out");
        nodes.Add(endNode);
        foreach (var node in nodes)
        {
            if (!childrenTempDict.TryGetValue(node.Id, out var children)) continue;
            var childNodes = children.Select(child => nodes.FirstOrDefault(n => n.Id == child));
            node.Children = [.. node.Children.Union(childNodes ?? [])!];
        }

        var dacNode = nodes.First(n => n.Id == "dac");
        var fftNode = nodes.First(n => n.Id == "fft");

        var memoizationCache = new Dictionary<string, long>();

        var result = RunToOut(nodes.First(node => node.Id == "svr"), false, false);

        return result;

        long RunToOut(Node node, bool visitedDac, bool visitedFft)
        {
            if (node == endNode && visitedDac && visitedFft) return 1;
            if (node == dacNode) visitedDac = true;
            if (node == fftNode) visitedFft = true;

            //check cache
            var cacheKey = $"{node.Id}{visitedDac}{visitedFft}";
            if (memoizationCache.TryGetValue(cacheKey, out var cacheVal))
            {
                return cacheVal;
            }

            long sum = 0;
            foreach (var child in node.Children)
            {
                sum += RunToOut(child, visitedDac, visitedFft);
            }
            memoizationCache.Add(cacheKey, sum);
            return sum;
        }
    }
    record Node(string Id)
    {
        public Node[] Children = [];
    }
}