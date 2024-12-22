using CommonLib;
using Spectre.Console;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var directions = new Dictionary<char, (int xOff, int yOff)>() {
            { '>', (1, 0) },
            { 'v', (0, 1) },
            { '^', (0, -1) },
            { '<', (-1, 0) }
        };

        var directionalKeypad = @".^A
                                  <v>".Split(Environment.NewLine).Select(s => s.Trim()).CreateGrid();
        var numericKeypad = @"789
                              456
                              123
                              .0A".Split(Environment.NewLine).Select(s => s.Trim()).CreateGrid();

        var codes = input;

        var robots = new List<Robot>()
        {
            new() {
                Grid = numericKeypad,
                Position = GetPositionOfKey('A', numericKeypad)
            },
            new() {
                Grid = directionalKeypad,
                Position = GetPositionOfKey('A', directionalKeypad)
            },
            new() {
                Grid = directionalKeypad,
                Position = GetPositionOfKey('A', directionalKeypad)
            }
        };

        List<char> GetDownstreamMoves((int x, int y) position, char targetKey, List<(int x, int y)> history, List<char> moves, Robot robot)
        {
            if (!GridTools.IsInGrid(position, robot.Grid))
            {
                return null;
            }
            var key = robot.Grid[position.y][position.x];
            if (key == '.' || history.Contains(position))
            {
                return null;
            }
            history.Add(position);
            if (key == targetKey)
            {
                moves.Add('A');
                robot.Position = position;
                if (robots.IndexOf(robot) == robots.Count - 1)
                {
                    return moves;
                }
                var nextRobot = robots[robots.IndexOf(robot)+1];
                var subRobotMoves = moves.Select(m =>
                {
                    return GetDownstreamMoves(nextRobot.Position, m, [], [], nextRobot);
                });
                return subRobotMoves.SelectMany(srm => srm).ToList();
            }
            var pathsToGoal = directions.Select(dir =>
            {
                var mCopy = moves.ToList();
                mCopy.Add(dir.Key);
                return GetDownstreamMoves(GridTools.Offset(position, dir.Value), targetKey, [.. history], mCopy, robot);
            });
            return pathsToGoal.Where(p => p != null).OrderBy(p => p.Count).FirstOrDefault();
        }
        var result = 0;
        foreach (var code in codes)
        {
            var outputs = new List<char>();
            foreach (var key in code)
            {
                outputs.AddRange(GetDownstreamMoves(robots.First().Position, key, [], [], robots.First()));
            }
            Console.WriteLine(string.Join("", outputs));
            Console.WriteLine($"{outputs.Count}*{int.Parse(code[..3])}");
            result += (outputs.Count * int.Parse(code[..3]));
        }
        Console.WriteLine(result);
        (int x, int y) GetPositionOfKey(char key, List < List<char> > grid)
        {
            var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
            return coordinates.First(c => grid[c.y][c.x] == key);
        }
    }
    class Robot
    {
        public (int x, int y) Position;
        public List<List<char>> Grid;
    }
}