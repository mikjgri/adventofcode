public class Task1_loop(string[] input)
{
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
    public void Solve()
    {
        var pipeMap = GetPipeMap();
        var startY = pipeMap.IndexOf(pipeMap.FirstOrDefault(line => line.Any(x => x == "S")));
        var startX = pipeMap[startY].IndexOf("S");

        var results = _directions.Keys.Select(dir =>
        {
            int[] pos = [startX, startY];
            int[] prevPos = [];
            int step = 0;
            while (true)
            {
                if (pos[0] < 0 || pos[0] > pipeMap[0].Count - 1 || pos[1] < 0 || pos[1] > pipeMap.Count - 1)
                {
                    return 0;
                }
                var instruction = step == 0 ? dir : pipeMap[pos[1]][pos[0]];
                if (instruction == ".")
                {
                    return 0;
                }
                if (instruction == "S")
                {
                    return step;
                }
                int[] nextDirection;
                if (prevPos.Length == 0)
                {
                    nextDirection = _directions[instruction][0];
                }
                else
                {
                    var backTrace = _directions[instruction].FirstOrDefault(dir => pos[0] + dir[0] == prevPos[0] && pos[1] + dir[1] == prevPos[1]);
                    if (backTrace == null)
                    {
                        return 0;
                    }
                    nextDirection = _directions[instruction].First(dir => dir != backTrace);
                }
                prevPos = pos;
                pos = [pos[0] + nextDirection[0], pos[1] + nextDirection[1]];
                step++;
            }
        });

        Console.WriteLine(results.FirstOrDefault(r => r != 0) / 2);
    }
}