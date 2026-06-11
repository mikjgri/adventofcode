using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var tree = new Dictionary<string, HashSet<string>>();

        foreach (var line in input)
        {
            var s = line.Split("-");
            tree.TryAdd(s[0], []);
            tree.TryAdd(s[1], []);
            tree[s[0]].Add(s[1]);
            tree[s[1]].Add(s[0]);
        }

        int WalkTheWalk(string currentCave, List<string> walkedPath)
        {
            if (currentCave == "end") return 1;
            if (char.IsLower(currentCave, 0) && walkedPath.Contains(currentCave)) return 0;
            walkedPath.Add(currentCave);
            var connectedNodes = tree[currentCave];
            return connectedNodes.Sum(cn => WalkTheWalk(cn, [.. walkedPath]));
        }

        return WalkTheWalk("start", []);
    }
}