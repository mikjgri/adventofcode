using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
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

        int WalkTheWalk(string currentCave, List<string> walkedPath, bool doubleVisitUsed = false)
        {
            if (currentCave == "end") return 1;
            if (walkedPath.Count != 0 && currentCave == "start") return 0;
            if (char.IsLower(currentCave, 0) && walkedPath.Contains(currentCave))
            {
                if (doubleVisitUsed)
                {
                    return 0;
                }
                doubleVisitUsed = true;
            }
            walkedPath.Add(currentCave);
            var connectedNodes = tree[currentCave];
            return connectedNodes.Sum(cn => WalkTheWalk(cn, [.. walkedPath], doubleVisitUsed));
        }

        return WalkTheWalk("start", []);
    }
}