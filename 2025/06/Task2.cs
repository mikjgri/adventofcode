using CommonLib;
using System.Data;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var rotatedInput = new string[input[0].Length];
        var operators = new List<char>();
        foreach (var line in input)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '*' || line[i] == '+')
                {
                    operators.Add(line[i]);
                }
                else
                {
                    rotatedInput[i] += line[i];
                }
            }
        }

        var dataTable = new DataTable();
        var sums = new List<long>();
        var lineIndex = 0;
        while (lineIndex < rotatedInput.Length - 1)
        {
            var numbers = rotatedInput[lineIndex..].TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim());
            lineIndex += numbers.Count() + 1;

            var expression = string.Join(operators[sums.Count], numbers.Select(n => $"{n}.0"));
            sums.Add(Convert.ToInt64(dataTable.Compute(expression, "")));
        }

        return sums.Sum();
    }
}