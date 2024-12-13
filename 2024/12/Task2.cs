using CommonLib;

public class Task2(string[] input)
{

    //DERP DERP, NO RUN
    public void Solve()
    {
        var grid = input.CreateGrid();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var squareDirections = GridTools.GetSquare4DirectionOffsets().ToList();
        var dir8 = GridTools.Get8DirectionOffsets();

        var handledCoordinates = new List<(int x, int y)>();

        var gardens = new List<FlowerGarden>();
        foreach (var coord in coordinates)
        {
            if (gardens.Any(garden => garden.Region.Contains(coord))) continue;
            gardens.Add(GetGarden(coord, grid[coord.y][coord.x], new FlowerGarden()));
        }

        foreach (var garden in gardens)
        {
            var corners = 0;
            foreach (var spot in garden.Region)
            {
                var left = ((spot.x - 1, spot.y));
                var containsLeft = garden.Region.Contains(left);
                var right = ((spot.x + 1, spot.y));
                var containsRight = garden.Region.Contains(right);
                var up = ((spot.x, spot.y - 1));
                var containsUp = garden.Region.Contains(up);
                var down = ((spot.x, spot.y + 1));
                var containsDown = garden.Region.Contains(down);

                var containsLeftAndRight = containsLeft && containsRight;
                var containsUpAndDown = containsUp && containsDown;

                if (containsLeftAndRight || containsUpAndDown) //not corner
                {
                    continue;
                }

                var neighbours = squareDirections.Where(dir => garden.Region.Contains((spot.x + dir.xOff, spot.y + dir.yOff))).ToList();
                if (neighbours.Count == 0)
                {
                    corners += 4;
                    continue;
                }
                
                if (a == 1)
                {
                    corners += 2;
                }
                else
                {
                    corners ++;
                }
                var b = 123;
            }
            Console.WriteLine(corners);
        }

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
                squareDirections.ToList().ForEach(dir => GetGarden((pos.x + dir.xOff, pos.y + dir.yOff), plantType, new FlowerGarden() { Region = garden.Region, Perimeter = garden.Perimeter }));
            }
            return garden;
        }
    }
    class FlowerGarden
    {
        public List<(int x, int y)> Perimeter = [];
        public List<(int x, int y)> Region = [];
    }
    class Traverse
    {
        public (int x, int y) Fence;
        public (int xOff, int yOff) Direction;
    }
}