public class Task2_bruteforce
{
    private string[] _input;
    public Task2_bruteforce(string[] input)
    {
        _input = input;
    }
    private Dictionary<string, (string left, string right)> GetMap()
    {
        return _input[2..].Select(line =>
        {
            var split = line.Replace(" ", "").Split("=");
            var currentLocation = split[0];
            var routes = split[1].Replace("(", "").Replace(")", "").Split(",");
            return (currentLocation, (routes[0], routes[1]));
        }).ToDictionary();
    }
    public void Solve()
    {
        var instructions = _input[0];
        var map = GetMap();

        double step = 0;
        var locations = map.Select(x => x.Key).Where(item => item.EndsWith("A")).ToList();

        while (!locations.All(x => x.EndsWith("Z")))
        {
            var instruction = instructions[(int)(step % instructions.Length)].ToString();

            var newLocs = new List<string>();

            foreach (var location in locations)
            {
                newLocs.Add(instruction == "L" ? map[location].left : map[location].right);
            }
            locations = newLocs;
            if (step % 10000000 == 0) Console.WriteLine(step);
            step++;
        }
        Console.WriteLine(step);
    }
}