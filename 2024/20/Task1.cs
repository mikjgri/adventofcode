using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input.CreateGrid();

        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var directions = GridTools.GetSquare4DirectionOffsets();
        var start = coordinates.First(c => grid[c.y][c.x] == 'S');

        var uncheated = MoveToEnd(start, 0, []);
        var maxRouteTime = uncheated - 100;

        var result = 0;
        foreach (var wall in coordinates.Where(c => grid[c.y][c.x] == '#'))
        {
            var pathsInVicinity = directions.Where(dir =>
            {
                var pos = GridTools.Offset(wall, dir);
                return GridTools.IsInGrid(pos, grid) && grid[pos.y][pos.x] != '#';
            }).Count();
            if (pathsInVicinity < 2)
            {
                continue;
            }
            var shortest = MoveToEnd(start, 0, [], maxRouteTime, wall);
            if (shortest != null) {
                result++;
            }
        }
        Console.WriteLine(result);

        int? MoveToEnd((int x, int y) pos, int time, Dictionary<string, int> memoization, int? bingoTime = null, (int x, int y)? cheatedPos = null)
        {
            var key = pos.ToString();
            if (!GridTools.IsInGrid(pos, grid) || (memoization.TryGetValue(key, out var positionCache) && positionCache < time) || (bingoTime != null && time > bingoTime))
            {
                return null;
            }
            var squareVal = grid[pos.y][pos.x];
            if (squareVal == 'E')
            {
                return time;
            }
            if (squareVal == '#' && cheatedPos != pos)
            {
                return null;
            }
            memoization[key] = time;
            var subPaths = directions.Select(dir => MoveToEnd((pos.x + dir.xOff, pos.y + dir.yOff), time + 1, memoization, bingoTime, cheatedPos)).Where(sp => sp != null);
            return subPaths.Min() ?? null;
        }
    }
}