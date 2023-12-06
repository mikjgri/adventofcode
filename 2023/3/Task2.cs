public class Task2
{
    private string[] _input;
    public Task2(string[] input)
    {
        _input = input;
    }
    private (string key, int number)? GetNumberInPosition(int lineNumber, int columnNumber)
    {
        if (lineNumber < 0 || columnNumber < 0) return null;
        if (lineNumber > _input.Length || columnNumber > _input[0].Length) return null;
        var line = _input[lineNumber];
        var c = line[columnNumber].ToString();
        if (int.TryParse(c, out _))
        {
            var start = GetEdge(columnNumber, line, -1);
            var end = GetEdge(columnNumber, line, 1);
            var number = line.Substring(start, end - start + 1);
            return ($"{lineNumber}-{start}", int.Parse(number));
        }
        return null;
    }
    private int GetEdge(int column, string line, int direction)
    {
        if (column < 0 || column > line.Length - 1 || !int.TryParse(line[column].ToString(), out _))
        {
            return column + direction * -1;
        }
        return GetEdge(column + direction, line, direction);
    }
    public void Solve()
    {
        var positions = new List<(int x, int y)>() { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) };

        var result = 0;
        for (var i = 0; i < _input.Length; i++)
        {
            var line = _input[i];
            for (var j = 0; j < line.Length; j++)
            {
                var str = line[j].ToString();
                if (str == "." || int.TryParse(str, out _))
                {
                    continue;
                }

                //hit symbol
                var partNumbers = new Dictionary<string, int>();
                foreach (var pos in positions)
                {
                    var number = GetNumberInPosition(i + pos.y, j + pos.x);
                    if (number.HasValue && !partNumbers.ContainsKey(number.Value.key))
                    {
                        partNumbers.Add(number.Value.key, number.Value.number);
                    }
                }
                if (partNumbers.Count == 2)
                {
                    var partValues = partNumbers.Values.ToList();
                    result += (partValues[0] * partValues[1]);
                }
            }
        }
        Console.WriteLine(result);
    }
}