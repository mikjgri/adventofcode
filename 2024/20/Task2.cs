using CommonLib;
using System;

public class Task2(string[] input) : BaseTask()
{
    private readonly int _cheatLength = 20;
    private readonly int _saveRequirement = 100;
    protected override void Solve()
    {
        var grid = input.CreateGrid();

        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var directions = GridTools.GetSquare4DirectionOffsets();
        var start = coordinates.First(c => grid[c.y][c.x] == 'S');

        var route = MoveToEnd(start, []).SelectWithIndex().ToList();
        var maxSteps = route.Count - _saveRequirement;

        Console.WriteLine(route.Sum(position =>
        {
            var future = route.Where(r => r.index > position.index).ToList();
            var inRange = future.Where(r => GridTools.GetManhattanDistance(r.item, position.item) <= _cheatLength).ToList();
            var validJump = inRange.Where(r => (position.index + GridTools.GetManhattanDistance(position.item, r.item) + (route.Count - r.index)) <= maxSteps).ToList();
            return validJump.Count;
        }));

        List<(int x, int y)> MoveToEnd((int x, int y) pos, List<(int x, int y)> history)
        {
            var key = pos.ToString();
            if (!GridTools.IsInGrid(pos, grid) || history.Contains(pos))
            {
                return null;
            }
            var squareVal = grid[pos.y][pos.x];
            if (squareVal == '#')
            {
                return null;
            }
            history.Add(pos);
            if (squareVal == 'E')
            {
                return history;
            }
            var subPaths = directions.Select(dir => MoveToEnd((pos.x + dir.xOff, pos.y + dir.yOff), [.. history]));
            return subPaths.First(sp => sp != null);
        }
    }
}