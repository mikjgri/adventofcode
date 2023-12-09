public class Task2
{
    private string[] _input;
    public Task2(string[] input)
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

        var locations = map.Select(x => x.Key).Where(item => item.EndsWith("A")).ToList();

        var stepsRequired = locations.Select(location =>
        {
            var loc = location;
            double step = 0;
            while (!loc.EndsWith("Z"))
            {
                var instruction = instructions[((int)step % instructions.Length)].ToString();
                loc = instruction == "L" ? map[loc].left : map[loc].right;
                step++;
            }
            return step;
        });

        var result = stepsRequired.Aggregate(LCM);
        Console.WriteLine(result);
    }

    private double GCF(double a, double b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    private double LCM(double a, double b)
    {
        return a / GCF(a, b) * b;
    }
}