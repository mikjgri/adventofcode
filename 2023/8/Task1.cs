using System.Security;

public class Task1
{
    private string[] _input;
    public Task1(string[] input)
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

        var step = 0;
        var location = "AAA";
        while(location != "ZZZ")
        {
            var instruction = instructions[step%instructions.Count];
            location = instruction == "L" ? map[location].left : map[location].right;
            step++;
        }
        Console.WriteLine(step);
    }
}