public class Task2(string[] input)
{
    public void Solve()
    {
        var directions = new List<(int xOff, int yOff)>()
        {
            (0,-1),
            (1,0),
            (0,1),
            (-1,0)
        };

        bool isLoop((int x, int y) obstacle)
        {
            var grid = input.Select(line => line.Select(c => c).ToList()).ToList();
            if (grid[obstacle.y][obstacle.x] == '.')
            {
                grid[obstacle.y][obstacle.x] = '#';
            }
            var pos = Enumerable.Range(0, grid.Count).SelectMany(y => Enumerable.Range(0, grid[0].Count).Select(x => (x, y))).FirstOrDefault(item => grid[item.y][item.x] == '^');
            var dir = directions[0];
            var posLog = new List<(int x, int y, int dirIndex)>();
            while (true)
            {
                if (posLog.Any(pL => pL.x == pos.x && pL.y == pos.y && directions.IndexOf(dir) == pL.dirIndex ))
                {
                    return true;
                }
                posLog.Add((pos.x, pos.y, directions.IndexOf(dir)));
                grid[pos.y][pos.x] = 'X';
                (int x, int y) nextPos = (pos.x + dir.xOff, pos.y + dir.yOff);
                if (nextPos.x < 0 || nextPos.x > grid[0].Count - 1 || nextPos.y < 0 || nextPos.y > grid.Count - 1) break;

                if (grid[nextPos.y][nextPos.x] == '#')
                {
                    dir = directions[(directions.IndexOf(dir) + 1) % directions.Count];
                }
                else
                {
                    pos = nextPos;
                }
            }
            return false;
        }
        Console.WriteLine(Enumerable.Range(0, input.Length).SelectMany(y => Enumerable.Range(0, input[0].Length).Select(x => (x, y))).Count(item => isLoop((item.x, item.y))));
    }
}