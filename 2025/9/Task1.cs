using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        List<(int x, int y)> coordinates = [.. input.Select(line =>
        {
            var s = line.Split(",");
            return (int.Parse(s[0]), int.Parse(s[1]));
        })];
        Console.WriteLine(coordinates.Max(c => coordinates.Max(oC => Math.Abs((long)c.x - oC.x + 1) * Math.Abs((long)c.y - oC.y + 1))));
    }
}
