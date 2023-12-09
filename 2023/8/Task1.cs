public class Task1
{
    private string[] _input;
    public Task1(string[] input)
    {
        _input = input;
    }
    private Dictionary<string, (string left, string right)> GetMap()
    {
        return _input[2..].Select(line =>
        {
            var split = line.Replace(" ","").Split("=");
            var currentLocation = split[0];
            var routes = split[1].Replace("(", "").Replace(")", "").Split(",");
            return (currentLocation, (routes[0], routes[1]));
        }).ToDictionary();
    }
    public void Solve()
    {
        var instructions = _input[0];
        var map = GetMap();

        var step = 0;
        var location = "AAA";
        while(location != "ZZZ")
        {
            var instruction = instructions[step%instructions.Length].ToString();
            location = instruction == "L" ? map[location].left : map[location].right;
            step++;
        }
        Console.WriteLine(step);
    }
}