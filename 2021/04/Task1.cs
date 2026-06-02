using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    private const int boardSize = 5;
    protected override object Solve()
    {
        var bingoNumbers = input[0].Split(",").Select(n => int.Parse(n)).ToList();
        var boards = GenerateBoards();

        foreach (var number in bingoNumbers)
        {
            foreach (var board in boards)
            {
                MarkDrawn(board, number);
                if (HasBingo(board))
                {
                    return CalculateBoardScore(board, number);
                }
            }
        }
        return -1;
    }
    List<List<List<(int number, bool drawn)>>> GenerateBoards()
    {
        var boards = new List<List<List<(int, bool)>>>();
        for (var i = 2; i < input.Length - 2; i += boardSize + 1)
        {
            var board = new List<List<(int, bool)>>();
            var boardLines = input[i..(i + boardSize)];
            board.AddRange(boardLines.Select(bl => bl.Split(" ").Where(n => n.Trim() != string.Empty).Select(n => (int.Parse(n), false)).ToList()));
            boards.Add(board);
        }
        return boards;
    }
    bool HasBingo(List<List<(int number, bool drawn)>> board)
    {
        for (var i = 0; i < boardSize; i++)
        {
            if (board[i].All(n => n.drawn)) return true; //horizontal bingo

            var verticalBingo = true;
            for (var j = 0; j < boardSize; j++)
            {
                if (!board[j][i].drawn)
                {
                    verticalBingo = false;
                    break;
                }
            }
            if (verticalBingo) return true;
        }
        return false;
    }
    void MarkDrawn(List<List<(int number, bool drawn)>> board, int number)
    {
        for (var row = 0; row < boardSize; row++)
        {
            for (var col = 0; col < boardSize; col++)
            {
                var elem = board[row][col];
                if (elem.number == number)
                {
                    elem.drawn = true;
                    board[row][col] = elem;
                }
            }
        }
    }
    int CalculateBoardScore(List<List<(int number, bool drawn)>> board, int winningNumber)
    {
        var unmarkedNumbersSum = board.Sum(line => line.Where(n => !n.drawn).Sum(n => n.number));
        return unmarkedNumbersSum * winningNumber;
    }
}