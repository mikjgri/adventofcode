using System.Security;

public class Task2
{
    private string[] _input;
    public Task2(string[] input)
    {
        _input = input;
    }
    private Dictionary<string, (string left, string right)> GetMap()
    {
        var ret = new Dictionary<string, (string left, string right)>();
        foreach (var line in _input[2..])
        {
            var split = line.Split("=");
            var currentLocation = split[0].Trim();
            var routes = split[1].Replace("(", "").Replace(")", "").Trim().Split(",");

            ret.Add(currentLocation, (routes[0].Trim(), routes[1].Trim()));
        }
        return ret;
    }
    public void Solve()
    {
        var instructions = _input[0].Select(x => x.ToString()).ToList();
        var map = GetMap();

        double step = 0;
        var locations = map.Select(x => x.Key).Where(item => item.EndsWith("A")).ToList();

        while (!locations.All(x => x.EndsWith("Z")))
        {
            var instruction = instructions[(int)(step % instructions.Count)];

            var newLocs = new List<string>();

            foreach (var location in locations)
            {
                newLocs.Add(instruction == "L" ? map[location].left : map[location].right);
            }
            locations = newLocs;
            step++;
        }
        Console.WriteLine(step);
    }
}