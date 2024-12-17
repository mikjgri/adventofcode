using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input.Select(line => line.Select(c => c).ToList()).ToList();

        var diagonalStartOffsets = new List<(int x, int y)>() { (1, 1), (1, -1) };

        Console.WriteLine(
            Enumerable.Range(0, grid[0].Count)
            .Sum(x => Enumerable.Range(0, grid.Count)
                .Count(y => getLetter(x, y) == 'A' && diagonalStartOffsets.TrueForAll(diagonalStart => checkUniqueDiagonally((x, y), diagonalStart, ['M', 'S'])))
            )
        );

        bool checkUniqueDiagonally((int x, int y) position, (int x, int y) offset, IEnumerable<char> letters)
        {
            if (!letters.Any()) return true;
            var letter = getLetter(position.x + offset.x, position.y + offset.y);
            return letter.HasValue && letters.Contains(letter.Value) && checkUniqueDiagonally(position, (offset.x * -1, offset.y * -1), letters.Where(l => l != letter.Value).ToList());
        }
        char? getLetter(int x, int y)
        {
            return (y < 0 || y > grid.Count - 1 || x < 0 || x > grid[0].Count - 1) ? null : grid[x][y];
        }
    }
}