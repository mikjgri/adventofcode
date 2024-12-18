using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    private int byteDropIndex = 1024;
    protected override void Solve()
    {
        List<(int x, int y)> bytePositions = input.Select(line => { var s = line.Split(","); return (int.Parse(s[0]), int.Parse(s[1])); }).ToList();

        (int x, int y) max = (bytePositions.Max(bp => bp.x), bytePositions.Max(bp => bp.y));
        var grid = GridTools.InitializeGrid(max.x + 1, max.y + 1, '.');
        var directions = GridTools.GetSquare4DirectionOffsets();

        foreach (var bytePosition in bytePositions.SelectWithIndex())
        {
            if (bytePosition.index > byteDropIndex) break;
            grid[bytePosition.item.y][bytePosition.item.x] = '#';
        }

        var log = new Dictionary<string, int>();
        while (RunForrestRun((0, 0), 0))
        {
            byteDropIndex++;
            grid[bytePositions[byteDropIndex].y][bytePositions[byteDropIndex].x] = '#';
            log = [];
        }
        Console.WriteLine(bytePositions[byteDropIndex]);
        bool RunForrestRun((int x, int y) position, int steps)
        {
            var key = $"{position}";
            if (!GridTools.IsInGrid(position, grid) || grid[position.y][position.x] == '#' || (log.ContainsKey(key) && log[key] <= steps))
            {
                return false;
            }
            log[key] = steps;
            if (position == max)
            {
                return true;
            }
            return directions.Any(dir => RunForrestRun((position.x + dir.xOff, position.y + dir.yOff), steps + 1));
        }
    }
}