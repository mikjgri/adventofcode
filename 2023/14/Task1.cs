using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    const char RollingStone = 'O';
    const char EmptySpace = '.';
    protected override object Solve()
    {
        var grid = input.CreateGrid();

        TiltBoard();

        return GridTools.GenerateCoordinates(grid[0].Count, grid.Count).Where(coord => grid[coord.y][coord.x] == RollingStone).Sum(coord => grid.Count-coord.y);

        void TiltBoard()
        {
            var movement = false;

            for (var x = 0; x < grid[0].Count; x++)
            {
                for (var y = 1; y < grid.Count; y++)
                {
                    if (grid[y][x] == RollingStone)
                    {
                        if (grid[y-1][x] == EmptySpace)
                        {
                            grid[y][x] = EmptySpace;
                            grid[y-1][x] = RollingStone;
                            movement = true;
                        }
                    }
                }
            }

            if (movement) TiltBoard();
        }
    }
}