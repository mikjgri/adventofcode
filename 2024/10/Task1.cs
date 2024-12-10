using CommonLib;

public class Task1(string[] input)
{
    public void Solve()
    {
        var grid = input.Select(line => line.Select(c => int.Parse(c.ToString())).ToList()).ToList();
        var directions = GridTools.Get4DirectionOffsets();

        Console.WriteLine(GridTools.GenerateCoordinates(grid[0].Count, grid.Count).Where(pos => grid[pos.y][pos.x] == 0).Sum(pos => PathsToUniqueTops(pos, 0, []).Count));
        
        List<(int x, int y)> PathsToUniqueTops((int x, int y) position, int expectedPosValue, List<(int x, int y)> trails)
        {
            if (!GridTools.IsInGrid(position, grid) || grid[position.y][position.x] != expectedPosValue) return trails;
            if (expectedPosValue == 9)
            {
                trails.Add(position);
                return trails;
            }
            return directions.Select(dir => PathsToUniqueTops((position.x + dir.xOff, position.y + dir.yOff), expectedPosValue + 1, [.. trails])).SelectMany(res => res).Distinct().ToList();
        }
    }
}