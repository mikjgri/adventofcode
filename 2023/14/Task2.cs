using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    const char RollingStone = 'O';
    const char EmptySpace = '.';
    const int TargetCycles = 1000000000;
    protected override object Solve()
    {
        var grid = input.CreateGrid();

        var seen = new Dictionary<int, int>();
        for (var i = 0; i < TargetCycles; i++)
        {
            SpinCycle();

            var gridHash = Hash(grid);

            if (seen.TryGetValue(gridHash, out var previousIndex))
            {
                var cycleLength = i - previousIndex;
                var remainingCycles = TargetCycles - i - 1;
                var cyclesToRun = remainingCycles % cycleLength;

                for (var j = 0; j < cyclesToRun; j++)
                {
                    SpinCycle();
                }

                return GridTools.GenerateCoordinates(grid[0].Count, grid.Count).Where(coord => grid[coord.y][coord.x] == RollingStone).Sum(coord => grid.Count - coord.y);
            }

            seen.Add(gridHash, i);
        }
        return -1; //not found :(

        void SpinCycle()
        {
            TiltBoard(0, -1); // north
            TiltBoard(-1, 0); // west
            TiltBoard(0, 1);  // south
            TiltBoard(1, 0);  // east
        }

        void TiltBoard(int xDiff, int yDiff)
        {
            var movement = false;

            for (var x = 0; x < grid[0].Count; x++)
            {
                for (var y = 0; y < grid.Count; y++)
                {
                    if (grid[y][x] == RollingStone)
                    {
                        (int x, int y) newGrid = (x + xDiff, y + yDiff);

                        if (GridTools.IsInGrid(newGrid, grid) && grid[newGrid.y][newGrid.x] == EmptySpace)
                        {
                            grid[y][x] = EmptySpace;
                            grid[newGrid.y][newGrid.x] = RollingStone;
                            movement = true;
                        }
                    }
                }
            }

            if (movement) TiltBoard(xDiff, yDiff);
        }
    }
    static int Hash(List<List<char>> grid)
    {
        var hash = new HashCode();

        foreach (var row in grid)
        {
            foreach (var c in row)
            {
                hash.Add(c);
            }
                

            hash.Add('\n');
        }
        return hash.ToHashCode();
    }
}