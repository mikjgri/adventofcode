using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input.Select(line => line.Select(c => c).ToList()).ToList();

        var directions = new List<(int xOff, int yOff)>()
        {
            (0,-1),
            (1,0),
            (0,1),
            (-1,0)
        };

        var pos = Enumerable.Range(0, grid.Count).SelectMany(y => Enumerable.Range(0, grid[0].Count).Select(x => (x, y))).FirstOrDefault(item => grid[item.y][item.x] == '^');
        var dir = directions[0];
        while (true)
        {
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
        Console.WriteLine(Enumerable.Range(0, grid.Count).SelectMany(y => Enumerable.Range(0, grid[0].Count).Select(x => (x, y))).Count(item => grid[item.y][item.x] == 'X'));
    }
}