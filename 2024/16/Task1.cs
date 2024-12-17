using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input.CreateGrid();

        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var start = coordinates.First(c => grid[c.y][c.x] == 'S');

        var directions = new Dictionary<Direction, (int xOff, int yOff)>() {
            { Direction.North, (0, -1) },
            { Direction.East, (1, 0) },
            { Direction.South, (0, 1) },
            { Direction.West, (-1, 0) }
        };
        var waysForwardForDirection = Enum.GetValues<Direction>() //pre-caching all possible directions
            .ToDictionary(dir1 => dir1, dir1 => Enum.GetValues<Direction>()
            .Where(dir2 => Math.Abs((int)dir1 - (int)dir2) != 2)
            .ToList()
        );
        var positionLog = new Dictionary<string, long>();

        Console.WriteLine(move(start, Direction.East, 0));
        long? move((int x, int y) position, Direction facingDirection, long score)
        {
            var key = $"{position}-{facingDirection}";
            if (positionLog.TryGetValue(key, out var res) && res <= score) return null;
            if (grid[position.y][position.x] == '#')
            {
                return null;
            }
            if (grid[position.y][position.x] == 'E')
            {
                return score;
            }
            positionLog[key] = score;
            var waysForward = waysForwardForDirection[facingDirection].Select(dir => move((position.x + directions[dir].xOff, position.y + directions[dir].yOff), dir, facingDirection == dir ? score + 1 : score + 1001)).ToList();
            return waysForward.Where(way => way.HasValue).Min();
        }
    }
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}