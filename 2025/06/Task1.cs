using CommonLib;
using System.Data;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var ti = input.Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(r => r).ToList()).ToList();

        var dataTable = new DataTable();
        long sum = 0;
        for (var c = 0; c < ti.First().Count; c++)
        {
            var expression = string.Join(ti[^1][c], Enumerable.Range(0, ti.Count-1).Select(i => $"{ti[i][c]}.0"));
            sum += Convert.ToInt64(dataTable.Compute(expression, ""));
        }
        Console.WriteLine(sum);
    }
}