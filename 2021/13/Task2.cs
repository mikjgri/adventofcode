using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<(int x, int y)> positions = [.. input.TakeWhile(l => !string.IsNullOrEmpty(l)).Select(l =>
        {
            var s = l.Split(",");
            return (int.Parse(s[0]), int.Parse(s[1]));
        })];
        List<(string axis, int value)> instructions = [.. input[(positions.Count+1)..].Select(l =>
        {
            var s = l.Split("fold along", StringSplitOptions.TrimEntries)[1].Split("=");
            return (s[0], int.Parse(s[1]));
        })];

        foreach (var (axis, value) in instructions)
        {
            positions = [.. positions.Select(pos =>
            {
                if (axis == "x")
                {
                    if (pos.x > value)
                    {
                        pos.x -= (pos.x - value) * 2;
                    }
                }
                else
                {
                    if (pos.y > value)
                    {
                        pos.y -= (pos.y - value) * 2;
                    }
                }
                return pos;
            }).Distinct()];
        }

        var grid = GridTools.InitializeGridList(positions.Max(p => p.x) + 1, positions.Max(p => p.y) + 1, ".");

        foreach (var (x, y) in positions)
        {
            grid[y][x] = "#";
        }
        grid.Visualize();

        return -1;
    }
}