using CommonLib;

public class Task2(string[] input) : BaseTask()
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
        var bestAtPos = new Dictionary<string, long>();

        var ranToEnd = new List<(long score, List<(int x, int y)> log)>();

        move(start, Direction.East, 0, []);

        var minScore = ranToEnd.Min(rte => rte.score);
        var uniquePositions = ranToEnd.Where(rte => rte.score == minScore).SelectMany(c => c.log).Distinct().Count();

        Console.WriteLine(uniquePositions);
        void move((int x, int y) position, Direction facingDirection, long score, List<(int x, int y)> log)
        {
            var key = $"{position}-{facingDirection}";
            if (bestAtPos.TryGetValue(key, out var res) && res < score) return;
            if (grid[position.y][position.x] == '#')
            {
                return;
            }
            log.Add(position);
            if (grid[position.y][position.x] == 'E')
            {
                ranToEnd.Add((score, log));
            }
            bestAtPos[key] = score;
            foreach (var dir in waysForwardForDirection[facingDirection])
            {
                move((position.x + directions[dir].xOff, position.y + directions[dir].yOff), dir, facingDirection == dir ? score + 1 : score + 1001, [.. log]);
            }
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