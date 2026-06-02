using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    private const int boardSize = 5;
    protected override object Solve()
    {
        var bingoNumbers = input[0].Split(",").Select(n => int.Parse(n)).ToList();
        var boards = GenerateBoards();

        foreach (var number in bingoNumbers)
        {
            foreach (var board in boards.Where(b => !b.HasWon))
            {
                MarkDrawn(board, number);
                if (HasBingo(board))
                {
                    board.HasWon = true;
                    if (!boards.Any(b => !b.HasWon)) //last winner
                    {
                        return CalculateBoardScore(board, number);
                    }
                }
            }
        }
        return -1;
    }
    List<Board> GenerateBoards()
    {
        var boards = new List<Board>();
        for (var i = 2; i < input.Length - 2; i += boardSize + 1)
        {
            var numbers = new List<List<(int, bool)>>();
            var boardLines = input[i..(i + boardSize)];
            numbers.AddRange(boardLines.Select(bl => bl.Split(" ").Where(n => n.Trim() != string.Empty).Select(n => (int.Parse(n), false)).ToList()));
            boards.Add(new Board()
            {
                Numbers = numbers,
            });
        }
        return boards;
    }
    bool HasBingo(Board board)
    {
        for (var i = 0; i < boardSize; i++)
        {
            if (board.Numbers[i].All(n => n.drawn)) return true; //horizontal bingo

            var verticalBingo = true;
            for (var j = 0; j < boardSize; j++)
            {
                if (!board.Numbers[j][i].drawn)
                {
                    verticalBingo = false;
                    break;
                }
            }
            if (verticalBingo) return true;
        }
        return false;
    }
    void MarkDrawn(Board board, int number)
    {
        for (var row = 0; row < boardSize; row++)
        {
            for (var col = 0; col < boardSize; col++)
            {
                var elem = board.Numbers[row][col];
                if (elem.number == number)
                {
                    elem.drawn = true;
                    board.Numbers[row][col] = elem;
                }
            }
        }
    }
    int CalculateBoardScore(Board board, int winningNumber)
    {
        var unmarkedNumbersSum = board.Numbers.Sum(line => line.Where(n => !n.drawn).Sum(n => n.number));
        return unmarkedNumbersSum * winningNumber;
    }
    class Board()
    {
        public bool HasWon;
        public required List<List<(int number, bool drawn)>> Numbers;
    }
}