using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var grid = input.Select(row => row.Select(col => int.Parse(col.ToString())).ToList()).ToList();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);

        var offsets = GridTools.GetSquare4DirectionOffsets();

        var basins = new Dictionary<(int x, int y), List<(int x, int y)>>();
        var processedCoords = new List<(int x, int y)>();

        foreach (var coord in coordinates)
        {
            var basin = GetBasin(coord);
            if (basin.Any())
            {
                var lowestPoint = FindLowestPoint(basin);
                basins.TryAdd(lowestPoint, basin);
            }
        }
        var highestBasins = basins.OrderByDescending(basin => basin.Value.Count).Take(3).Select(basin => basin.Value.Count).ToList();
        return highestBasins[0] * highestBasins[1] * highestBasins[2];

        List<(int x, int y)> GetBasin((int x, int y) pos, List<(int x, int y)>? basin = default)
        {
            basin ??= [];
            if (!GridTools.IsInGrid(pos, grid) || basin.Contains(pos) || processedCoords.Contains(pos)) return basin;
            processedCoords.Add(pos);
            var gVal = grid[pos.y][pos.x];
            if (gVal == 9) return basin;
            basin.Add(pos);
            var subBasins = offsets.Select(oCoord => GetBasin((pos.x + oCoord.xOff, pos.y + oCoord.yOff), [.. basin]));
            var a = subBasins.SelectMany(basin => basin).Distinct().ToList();
            return a;
        }
        (int x, int y) FindLowestPoint(List<(int x, int y)> basin)
        {
            return basin.OrderBy(bCoord => grid[bCoord.y][bCoord.x]).First();
        }
    }
}