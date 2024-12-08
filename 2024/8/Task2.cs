using CommonLib;

public class Task2(string[] input)
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
                (int x, int y) diff = (secondPos.x - firstPos.x, secondPos.y - firstPos.y);

                void PlaceAntinodes((int x, int y) antinode)
                {
                    antinode = (antinode.x + (diff.x), antinode.y + (diff.y));
                    if (!GridTools.IsInGrid(antinode, antinodeGrid))
                    {
                        return;
                    }
                    antinodeGrid[antinode.y][antinode.x] = '#';
                    PlaceAntinodes(antinode);
                }
                PlaceAntinodes(firstPos);
            });
        });

        Console.WriteLine(antinodeGrid.Sum(row => row.Count(column => column == '#')));

        antinodeGrid.Visualize();
    }
}