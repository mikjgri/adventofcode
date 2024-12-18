using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    private readonly int byteDropCount = 1024;
    protected override void Solve()
    {
        List<(int x, int y)> bytePositions = input.SelectWithIndex().Where(line => line.index < byteDropCount).Select(line => { var s = line.item.Split(","); return (int.Parse(s[0]), int.Parse(s[1])); }).ToList();

        (int x, int y) max = (bytePositions.Max(bp => bp.x), bytePositions.Max(bp => bp.y));
        var grid = GridTools.InitializeGrid(max.x+1, max.y+1, '.');
        var directions = GridTools.GetSquare4DirectionOffsets();

        foreach (var bytePosition in bytePositions)
        {
            grid[bytePosition.y][bytePosition.x] = '#';
        }
        var log = new Dictionary<string, int>();
        Console.WriteLine(RunForrestRun((0, 0), 0));
        int? RunForrestRun((int x, int y) position, int steps)
        {
            var key = $"{position}";
            if (!GridTools.IsInGrid(position, grid) || grid[position.y][position.x] == '#' || (log.ContainsKey(key) && log[key] <= steps))
            {
                return null;
            }
            log[key] = steps;
            if (position == max)
            {
                return steps;
            }
            return directions.Select(dir => RunForrestRun((position.x+dir.xOff, position.y + dir.yOff), steps+1)).Where(r => r.HasValue).Min() ?? null;
        }
    }
}