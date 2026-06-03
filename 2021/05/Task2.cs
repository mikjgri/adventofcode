using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<((int x, int y) start, (int x, int y) end)> lines = [.. input.Select(line =>
        {
            var s = line.Split("->", StringSplitOptions.TrimEntries);

            static (int x, int y) getCoordinates(string p)
            {
                var s1 = p.Split(",");
                return (int.Parse(s1[0]), int.Parse(s1[1]));
            }

            return (getCoordinates(s[0]), getCoordinates(s[1]));
        })];

        var xSize = lines.Max(l => int.Max(l.start.x, l.end.x)) + 1;
        var ySize = lines.Max(l => int.Max(l.start.y, l.end.y)) + 1;

        var grid = GridTools.InitializeGridArray<int>(xSize, ySize, default);

        foreach (var (start, end) in lines)
        {
            var x = start.x;
            var y = start.y;
            grid[y][x]++;

            while (x != end.x || y != end.y)
            {
                if (x < end.x) x++;
                else if (x > end.x) x--;
                if (y < end.y) y++;
                else if (y > end.y) y--;
                grid[y][x]++;
            }
        }
        //grid.Visualize();

        return grid.Sum(row => row.Count(col => col > 1));
    }
}