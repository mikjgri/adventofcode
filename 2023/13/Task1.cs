using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var boards = new List<List<List<char>>>
        {
            new()
        };
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) boards.Add([]);
            else
            {
                boards.Last().Add([.. line.Select(c => c)]);
            }
        }

        var sum = boards.Sum(board =>
        {
            var horizontalLines = board.Select(line => string.Concat(line)).ToArray();
            var horizontalReflection = FindReflection(horizontalLines);
            if (horizontalReflection != null) return (int)Math.Ceiling(horizontalReflection.Value) * 100;

            var verticalLines = Enumerable.Range(0, board[0].Count).Select(x => string.Concat(board.Select(row => row[x]))).ToArray();
            var verticalReflection = FindReflection(verticalLines);
            if (verticalReflection != null) return (int)Math.Ceiling(verticalReflection.Value);

            return 0;
        });


        return sum;
    }

    double? FindReflection(string[] lines)
    {
        for (var i = 0.5; i < lines.Length - 1; i++)
        {
            if (lines[(int)Math.Floor(i)] == lines[(int)Math.Ceiling(i)]) //initial match, fan out check
            {
                var fanOutCheckCount = (int)Math.Floor(i < lines.Length / 2 ? i : lines.Length - i - 1);

                var mismatchFound = false;
                for (var j = 1; j <= fanOutCheckCount; j++)
                {
                    if (lines[(int)Math.Floor(i - j)] != lines[(int)Math.Ceiling(i + j)])
                    {
                        mismatchFound = true;
                        break;
                    }
                }
                if (!mismatchFound)
                {
                    return i;
                }
            }
        }
        return null;
    }
}