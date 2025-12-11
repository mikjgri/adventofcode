using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var grid = input.CreateGrid();
        var wasteland = new List<(int x, int y)>();
        var startPosition = GridTools.GenerateCoordinates(grid[0].Count, grid.Count).First(pos => grid[pos.y][pos.x] == 'S');

        var alderaansDestroyed = 0;
        FireAtWillCommander(startPosition);
        void FireAtWillCommander((int x, int y) pos)
        {
            if (!GridTools.IsInGrid(pos, grid) || wasteland.Contains(pos)) return;
            wasteland.Add(pos);
            var gridValue = grid[pos.y][pos.x];
            if (gridValue != '^')
            {
                FireAtWillCommander((pos.x, pos.y + 1));
            }
            else
            {
                alderaansDestroyed++;
                FireAtWillCommander((pos.x + 1, pos.y));
                FireAtWillCommander((pos.x - 1, pos.y));
            }

        }
        return alderaansDestroyed;
    }
}