using CommonLib;

public class Task2(string[] input) : BaseTask()
{

    //DERP DERP, NO RUN
    protected override void Solve()
    {
        var grid = input.CreateGrid();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var squareDirections = GridTools.GetSquare4DirectionOffsets().ToList();
        var diagonalDirections = GridTools.GetDiagonal4DirectionOffsets().ToList();

        var handledCoordinates = new List<(int x, int y)>();

        var gardens = new List<FlowerGarden>();
        foreach (var coord in coordinates)
        {
            if (gardens.Any(garden => garden.Region.Contains(coord))) continue;
            gardens.Add(GetGarden(coord, grid[coord.y][coord.x], new FlowerGarden()));
        }

        Console.WriteLine(gardens.Sum(garden =>
        {
            var corners = 0;
            foreach (var spot in garden.Region)
            {
                var containsLeft = garden.Region.Contains(((spot.x - 1, spot.y)));
                var containsRight = garden.Region.Contains(((spot.x + 1, spot.y)));
                var containsUp = garden.Region.Contains(((spot.x, spot.y - 1)));
                var containsDown = garden.Region.Contains(((spot.x, spot.y + 1)));
                var containsUpLeft = garden.Region.Contains(((spot.x - 1, spot.y - 1)));
                var containsUpRight = garden.Region.Contains(((spot.x + 1, spot.y - 1)));
                var containsDownLeft = garden.Region.Contains(((spot.x - 1, spot.y + 1)));
                var containsDownRight = garden.Region.Contains(((spot.x + 1, spot.y + 1)));

                var containsLeftAndRight = containsLeft && containsRight;
                var containsUpAndDown = containsUp && containsDown;

                //check outer
                var squareNeighbours = squareDirections.Where(dir => garden.Region.Contains((spot.x + dir.xOff, spot.y + dir.yOff))).ToList();
                if (containsLeftAndRight || containsUpAndDown)
                {
                    //not outer corner
                }
                else if (squareNeighbours.Count == 0)
                {
                    corners += 4; //single region
                }
                else if (squareNeighbours.Count == 1)
                {
                    corners += 2; //one neighbour must be two outer corners
                }
                else if (squareNeighbours.Count == 2)
                {
                    corners++; //two neighbours must be one outer corner
                }

                //check inner
                var diagonalNeighbours = diagonalDirections.Where(dir => garden.Region.Contains((spot.x + dir.xOff, spot.y + dir.yOff))).ToList();
                if (diagonalNeighbours.Count == 4)
                {
                    continue; //not inner corner
                }
                if (containsUp && containsRight && !containsUpRight)
                {
                    corners++;
                }
                if (containsRight && containsDown && !containsDownRight)
                {
                    corners++;
                }
                if (containsDown && containsLeft && !containsDownLeft)
                {
                    corners++;
                }
                if (containsLeft && containsUp && !containsUpLeft)
                {
                    corners++;
                }

            }
            return corners * garden.Region.Count;
        }));

        FlowerGarden GetGarden((int x, int y) pos, char plantType, FlowerGarden garden)
        {
            if (garden.Region.Contains(pos)) return garden;

            if (GridTools.IsInGrid(pos, grid) && grid[pos.y][pos.x] == plantType)
            {
                garden.Region.Add(pos);
                squareDirections.ToList().ForEach(dir => GetGarden((pos.x + dir.xOff, pos.y + dir.yOff), plantType, new FlowerGarden() { Region = garden.Region }));
            }
            return garden;
        }
    }
    class FlowerGarden
    {
        public List<(int x, int y)> Region = [];
    }
}
