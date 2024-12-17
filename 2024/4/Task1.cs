using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input.Select(line => line.Select(c => c).ToList()).ToList();

        var lookupWord = "XMAS";

        var directions = Enumerable.Range(-1, 3)
            .SelectMany(x => Enumerable.Range(-1, 3), (x, y) => (x, y))
            .Where(offset => offset != (0, 0))
            .ToList();


        Console.WriteLine(
            Enumerable.Range(0, grid[0].Count)
                .Sum(x => Enumerable.Range(0, grid.Count)
                    .Sum(y => directions
                        .Count(direction => hasWord(0, (x, y), direction))
                    )
                )
        );

        bool hasWord(int letterIndex, (int x, int y) position, (int x, int y) direction)
        {
            if (position.y < 0 || position.y > grid.Count - 1 || position.x < 0 || position.x > grid[0].Count - 1)
            {
                return false;
            }
            if (grid[position.y][position.x] == lookupWord[letterIndex])
            {
                return (letterIndex == lookupWord.Length - 1) || hasWord(letterIndex + 1, (position.x + direction.x, position.y + direction.y), direction);
            }
            return false;
        }
    }
}