using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var grid = input.Select(row => row.Select(col => int.Parse(col.ToString())).ToList()).ToList();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var offSet8d = GridTools.Get8DirectionOffsets();

        var i = 0;
        while (true)
        {
            i++;
            Dictionary<(int x, int y), bool> hasFlashed = [];
            foreach (var coord in coordinates)
            {
                hasFlashed.Add(coord, false);
                grid[coord.y][coord.x]++;
            }

            bool firstRun = true;
            bool someoneFlashed = false;
            while (firstRun || someoneFlashed)
            {
                someoneFlashed = false;
                firstRun = false;

                foreach (var coord in coordinates)
                {
                    if (hasFlashed[coord]) continue;

                    if (grid[coord.y][coord.x] > 9) //should flash
                    {
                        hasFlashed[coord] = true;
                        someoneFlashed = true;
                        foreach (var (xOff, yOff) in offSet8d)
                        {
                            (int x, int y) adjPos = (coord.x + xOff, coord.y + yOff);
                            if (!GridTools.IsInGrid(adjPos, grid)) continue; //out of bounds

                            grid[adjPos.y][adjPos.x]++;
                        }
                    }
                }
            }

            if (hasFlashed.Values.All(v => v)) //all flashed, happy
            {
                break;
            }

            foreach (var flashedOctupus in hasFlashed.Where(item => item.Value)) //set all that flashed to 0
            {
                grid[flashedOctupus.Key.y][flashedOctupus.Key.x] = 0;
            }
        }

        return i;
    }
}