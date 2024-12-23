using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var connections = input.Select(line => line.Split('-').Select(c => c.Trim()).ToList()).ToList();

        var keyConnections = new Dictionary<string, List<string>>();
        foreach (var l in connections)
        {
            foreach (var c in l)
            {
                if (keyConnections.ContainsKey(c))
                {
                    break;
                }
                var cConnections = new List<string>();
                foreach (var otherL in connections)
                {
                    if (otherL.Any(otherC => otherC == c))
                    {
                        cConnections.Add(otherL.First(otherC => otherC != c));
                    }
                }
                keyConnections.Add(c, cConnections);
            }
        }

        var lanparties = new List<List<string>>();
        foreach (var (key, neighbors) in keyConnections)
        {
            foreach (var neighbor in neighbors)
            {
                foreach (var mutualNeighbor in keyConnections[neighbor])
                {
                    if (mutualNeighbor != key && neighbors.Contains(mutualNeighbor))
                    {
                        var lanparty = new List<string> { key, neighbor, mutualNeighbor };
                        lanparty.Sort();
                        if (!lanparties.Any(lp => lanparty.SequenceEqual(lp)))
                        {
                            lanparties.Add(lanparty);
                        }
                    }
                }
            }
        }
        Console.WriteLine(lanparties.Count(lp => lp.Any(connection => connection[0] == 't')));

    }
}
