using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        // ^ = -1
        // . = 0
        // S = 1
        var grid = input.Select(row => row.Select(val => val == '^' ? (long)-1 : (val == 'S' ? 1 : 0)).ToList()).ToList();
        for (var y = 1; y < grid.Count; y++)
        {
            for (var x = 0; x < grid[0].Count; x++)
            {
                var valAbove = grid[y - 1][x];
                var thisVal = grid[y][x];
                if (valAbove == -1) continue;
                if (thisVal != -1)
                {
                    grid[y][x] = thisVal + valAbove;
                }
                else
                {
                    grid[y][x - 1] = grid[y][x - 1] + valAbove;
                    grid[y][x + 1] = grid[y][x + 1] + valAbove;
                }
            }
        }

        return Enumerable.Range(0, grid[0].Count).Sum(x => grid[^1][x]);
    }
}