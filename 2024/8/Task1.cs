using CommonLib;

public class Task1(string[] input)
{
    public void Solve()
    {
        var grid = input.CreateGrid();
        var antinodeGrid = GridTools.InitializeGrid(grid.Count, grid[0].Count, '.');
        var coordinates = GridTools.GenerateCoordinates(grid.Count, grid[0].Count);

        coordinates.Where(firstPos => grid[firstPos.y][firstPos.x] != '.').ToList().ForEach(firstPos =>
        {
            var posValue = grid[firstPos.y][firstPos.x];
            coordinates.Where(secondPos => firstPos != secondPos && grid[secondPos.y][secondPos.x] == posValue).ToList().ForEach(secondPos =>
            {
                (int x, int y) antinode = (secondPos.x + (secondPos.x - firstPos.x), secondPos.y + (secondPos.y - firstPos.y));

                if (GridTools.IsInGrid(antinode, antinodeGrid))
                {
                    antinodeGrid[antinode.y][antinode.x] = '#';
                }
            });
        });

        Console.WriteLine(antinodeGrid.Sum(row => row.Count(column => column == '#')));

        antinodeGrid.FancyVisualize();
    }
}