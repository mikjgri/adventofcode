using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input.CreateGrid();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var directions = GridTools.GetSquare4DirectionOffsets();

        var handledCoordinates = new List<(int x, int y)>();

        var gardens = new List<FlowerGarden>();
        foreach (var coord in coordinates)
        {
            if (gardens.Any(garden => garden.Region.Contains(coord))) continue;
            gardens.Add(GetGarden(coord, grid[coord.y][coord.x], new FlowerGarden()));
        }

        Console.WriteLine(gardens.Sum(garden => garden.Perimeter.Count * garden.Region.Count));

        FlowerGarden GetGarden((int x, int y) pos, char plantType, FlowerGarden garden)
        {
            if (garden.Region.Contains(pos)) return garden;

            if (!GridTools.IsInGrid(pos, grid) || grid[pos.y][pos.x] != plantType)
            {
                garden.Perimeter.Add(pos);
            }
            else
            {
                garden.Region.Add(pos);
                directions.ToList().ForEach(dir => GetGarden((pos.x + dir.xOff, pos.y + dir.yOff), plantType, new FlowerGarden() { Region = garden.Region, Perimeter = garden.Perimeter }));
            }
            return garden;
        }
    }
    class FlowerGarden
    {
        public List<(int x, int y)> Perimeter = [];
        public List<(int x, int y)> Region = [];
    }
}