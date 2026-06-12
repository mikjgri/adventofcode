using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var grid = input.CreateGrid(c => int.Parse(c.ToString()));
        (int x, int y) start = (0, 0);
        (int x, int y) end = (grid[0].Count - 1, grid.Count - 1);



        var safestWayToPos = new Dictionary<(int x, int y), int>();
        foreach (var coord in GridTools.GenerateCoordinates(end.x + 1, end.y + 1))
        {
            safestWayToPos.Add(coord, int.MaxValue);
        }

        var directionOffsets = GridTools.GetSquare4DirectionOffsets();
        var safestPath = FindSafestPath(start, (-1, -1), 0, true);

        int? FindSafestPath((int x, int y) pos, (int x, int y) prevPos, int accumulatedRisk, bool enter = false)
        {
            if (!GridTools.IsInGrid(pos, grid)) return null;
            if (!enter) accumulatedRisk += grid[pos.y][pos.x];
            if (safestWayToPos[end] <= accumulatedRisk) return null;
            if (safestWayToPos[pos] <= accumulatedRisk) return null;
            safestWayToPos[pos] = accumulatedRisk;
            if (pos == end)
            {
                //Console.WriteLine($"New best: {accumulatedRisk}");
                return accumulatedRisk;
            }

            int? safestPathRisk = null;

            var peakedDirectionOffsets = directionOffsets
            .Where(dir =>
            {
                var newPos = (pos.x + dir.xOff, pos.y + dir.yOff);
                return GridTools.IsInGrid(newPos, grid) && newPos != prevPos;
            })
            .OrderBy(dir => //lowest adjecent risk
            {
                var newPos = (pos.x + dir.xOff, pos.y + dir.yOff);
                return grid[newPos.Item2][newPos.Item1];
            })
            .OrderBy(dir => //manhattan distance
            {
                var newPos = (pos.x + dir.xOff, pos.y + dir.yOff);
                var manhattanDistance = GridTools.GetManhattanDistance(newPos, end);
                return manhattanDistance;
            })
            .ToList();

            foreach (var offset in peakedDirectionOffsets)
            {
                var sub = FindSafestPath((pos.x + offset.xOff, pos.y + offset.yOff), pos, accumulatedRisk);
                if (sub != null)
                {
                    if (safestPathRisk == null)
                    {
                        safestPathRisk = sub;
                    }
                    else if (sub < safestPathRisk)
                    {
                        safestPathRisk = sub;
                    }
                }
            }
            return safestPathRisk;

        }
        return safestPath;
    }
}