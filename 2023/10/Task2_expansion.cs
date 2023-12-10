public class Task2_expansion(string[] input)
{
    //WARNING: This solution is insanity...

    private string[] _input = input;
    private Dictionary<string, List<int[]>> _directions = new()
    { // x, y
        {"|", [[0,-1], [0,1]] },
        {"-", [[-1,0], [1,0]] },
        {"L", [[0,-1], [1,0]] },
        {"J", [[0,-1], [-1,0]] },
        {"7", [[0,1], [-1,0]] },
        {"F", [[0,1], [1,0]] },
    };

    private List<List<string>> GetPipeMap()
    {
        return _input.Select(line => line.Select(x => x.ToString()).ToList()).ToList();
    }
    private void AddExpanded(string[,] map, int[] pos, string expandedString)
    {
        for (var y = 0; y < 3; y++)
        {
            var line = expandedString.Split(Environment.NewLine)[y].Trim();
            for (var x = 0; x < 3; x++)
            {
                var newX = (pos[0] * 3) + x;
                var newY = (pos[1] * 3) + y;
                map[newX, newY] = line[x].ToString();
            }
        }
    }
    private string[,] GetExpandedPipeMap()
    {
        string[,] map = new string[_input[0].Length * 3, _input.Length * 3];

        for (var y = 0; y < _input.Length; y++)
        {
            for (var x = 0; x < _input[0].Length; x++)
            {
                var c = _input[y][x].ToString();
                switch (c)
                {
                    case "S":
                        AddExpanded(map, [x, y], @".|.
                                                -S-
                                                .|.");
                        break;
                    case "|":
                        AddExpanded(map, [x, y], @".|.
                                                .|.
                                                .|.");
                        break;
                    case "-":
                        AddExpanded(map, [x, y], @"...
                                                ---
                                                ...");
                        break;
                    case "L":
                        AddExpanded(map, [x, y], @".|.
                                                .L-
                                                ...");
                        break;
                    case "J":
                        AddExpanded(map, [x, y], @".|.
                                                -J.
                                                ...");
                        break;
                    case "7":
                        AddExpanded(map, [x, y], @"...
                                                -7.
                                                .|.");
                        break;
                    case "F":
                        AddExpanded(map, [x, y], @"...
                                                .F-
                                                .|.");
                        break;
                    case ".":
                        AddExpanded(map, [x, y], @"...
                                                ...
                                                ...");
                        break;
                    default:
                        break;
                }
            }
        }
        return map;
    }
    public void Solve()
    {
        var originalPipeMap = GetPipeMap();
        var pipeMapExpanded = GetExpandedPipeMap();
        var maxY = pipeMapExpanded.GetLength(0);
        var maxX = pipeMapExpanded.GetLength(1);

        var startY = originalPipeMap.IndexOf(originalPipeMap.FirstOrDefault(line => line.Any(x => x == "S")));
        var startX = originalPipeMap[startY].IndexOf("S");

        startX = (startX * 3) + 1;
        startY = (startY * 3) + 1;

        var results = _directions.Keys.Select(dir =>
        {
            var positions = new List<int[]>()
            {
                { [startX, startY] }
            };
            while (true)
            {
                var pos = positions.Last();
                if (pos[0] < 0 || pos[0] > maxX || pos[1] < 0 || pos[1] > maxY)
                {
                    return null;
                }
                var instruction = positions.Count == 1 ? dir : pipeMapExpanded[pos[0], pos[1]];
                if (instruction == ".")
                {
                    return null;
                }
                if (instruction == "S")
                {
                    return positions;
                }

                int[] nextDirection;
                if (positions.Count == 1)
                {
                    nextDirection = _directions[instruction][0];
                }
                else
                {
                    var prevPos = positions[^2];
                    var backTrace = _directions[instruction].FirstOrDefault(dir => pos[0] + dir[0] == prevPos[0] && pos[1] + dir[1] == prevPos[1]);
                    if (backTrace == null)
                    {
                        return null;
                    }
                    nextDirection = _directions[instruction].First(dir => dir != backTrace);
                }
                positions.Add([pos[0] + nextDirection[0], pos[1] + nextDirection[1]]);
            }
        });
        var loop = results.FirstOrDefault(r => r != null);

        var groundPositions = new List<int[]>();
        for (var y = 0; originalPipeMap.Count > y; y++)
        {
            for (var x = 0; originalPipeMap[0].Count > x; x++)
            {
                if (!loop.Any(l => l[0] == (x*3)+1 && l[1] == (y*3)+1))
                {
                    groundPositions.Add([(x * 3) + 1, (y * 3) + 1]);
                }
            }
        }

        int[,] loopGrid = new int[maxX, maxY]; //waaaaay quicker than List<int[]> to lookup in
        foreach (var pos in loop)
        {
            loopGrid[pos[0], pos[1]] = 1;
        }


        var pDir = new List<int[]>()
        {
            {[0,1]},
            {[0,-1]},
            {[1,0]},
            {[-1,0]}
        };

        bool canBreakOut((int x, int y) pos, int[,] history)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x > maxX - 1 || pos.y > maxY - 1)
            {
                return true;
            }
            if (history[pos.x, pos.y] == 1)
            {
                return false;
            }
            history[pos.x, pos.y] = 1;

            if (loopGrid[pos.x, pos.y] == 1)
            {
                return false;
            }
            return pDir.Any(p => canBreakOut((pos.x + p[0], pos.y + p[1]), history));
        }
        var breakOuts = groundPositions.Where(gp =>
        {
            int[,] bjarne = new int[maxX, maxY];
            return !canBreakOut((gp[0], gp[1]), bjarne);
        });

        Console.WriteLine(breakOuts.Count());
    }
}