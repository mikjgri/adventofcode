using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var endNode = new Node("out");

        //build node structure
        var childrenTempDict = new Dictionary<string,List<string>>();
        var nodes = input.Select(line =>
        {
            var split = line.Split(": ");
            childrenTempDict.Add(split[0],[.. split[1].Split(" ").Select(str => str)]);
            return new Node(split[0]);
        }).ToList();
        nodes.Add(endNode);
        foreach(var node in nodes)
        {
            if (!childrenTempDict.TryGetValue(node.Id, out var children)) continue;
            var childNodes = children.Select(child => nodes.FirstOrDefault(n => n.Id == child));
            node.Children = [.. node.Children.Union(childNodes ?? [])];
        }

        var result = 0;

        RunToOut(nodes.First(node => node.Id == "you"));

        return result;

        void RunToOut(Node node)
        {
            if (node == endNode)
            {
                result++;
                return;
            }
            foreach (var child in node.Children)
            {
                RunToOut(child);
            }
        }
    }
    record Node(string Id)
    {
        public Node[] Children = [];
    }
}