using CommonLib;
using Spectre.Console.Rendering;

public class Task2_Overengineered(string[] input) : BaseTaskV2()
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
            node.Children = [.. node.Children.Union(childNodes ?? [])];
        }

        var dacNode = nodes.First(n => n.Id == "dac");
        dacNode.CanLeadToDac = true;
        var fftNode = nodes.First(n => n.Id == "fft");
        fftNode.CanLeadToFft = true;

        //removing stupid nodes
        var orignalNodeCount = nodes.Count();
        var removedNode = true;
        while (removedNode)
        {
            removedNode = false;
            var nCopy = nodes.ToList();
            foreach (var node in nodes)
            {
                if (node == dacNode || node == fftNode) continue;
                if (node.Children.Count() == 1)
                {
                    foreach (var pNode in nCopy)
                    {
                        if (pNode.Children.Any(n => n == node))
                        {
                            pNode.Children.Replace(node, node.Children[0]);
                        }
                    }
                    nCopy.Remove(node);
                    removedNode = true;
                }
            }
            nodes = nCopy;
        }
        Console.WriteLine($"Optimized away {orignalNodeCount - nodes.Count()} nodes");

        foreach (var node in nodes)
        {
            node.CanLeadToDac = CanLeadTo(node, dacNode);
            node.CanLeadToFft = CanLeadTo(node, fftNode);
        }
        bool CanLeadTo(Node node, Node target)
        {
            if (target == dacNode)
            {
                if (node.CanLeadToDac.HasValue) return node.CanLeadToDac.Value;
                if (node == dacNode) return true;
            }
            else
            {
                if (node.CanLeadToFft.HasValue) return node.CanLeadToFft.Value;
                if (node == fftNode) return true;
            }
            var anyGood = false;
            foreach (var subNode in node.Children)
            {
                var ret = CanLeadTo(subNode, target);
                if (ret) anyGood = true;
                if (target == dacNode)
                {
                    subNode.CanLeadToDac = ret;
                }
                else
                {
                    subNode.CanLeadToFft = ret;
                }
            }
            return anyGood;
        }
        Console.WriteLine($"Preprocessed nodes. Can lead to; DAC = {nodes.Count(n => n.CanLeadToDac.Value)}, FFT = {nodes.Count(n => n.CanLeadToFft.Value)}");

        var cache = new Dictionary<string, long>();

        return(RunToOut(nodes.First(node => node.Id == "svr"), false, false));

        long RunToOut(Node node, bool visitedDac, bool visitedFft)
        {
            if (node == endNode && visitedDac && visitedFft) return 1;
            if (node == dacNode) visitedDac = true;
            if (node == fftNode) visitedFft = true;

            if (node.CanLeadToDac.HasValue && !visitedDac && !node.CanLeadToDac.Value) return 0;
            if (node.CanLeadToFft.HasValue && !visitedFft && !node.CanLeadToFft.Value) return 0;

            //check cache
            var cacheKey = $"{node.Id}{visitedDac}{visitedFft}";
            if (cache.TryGetValue(cacheKey, out var cacheVal))
            {
                return cacheVal;
            }

            long sum = 0;
            foreach (var child in node.Children)
            {
                sum += RunToOut(child, visitedDac, visitedFft);
            }
            cache.Add(cacheKey, sum);
            return sum;
        }
    }
    record Node(string Id)
    {
        public Node[] Children = [];
        public bool? CanLeadToDac;
        public bool? CanLeadToFft;
    }
}