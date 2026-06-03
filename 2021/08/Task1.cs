using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var res = 0;
        foreach (var item in input)
        {
            var s1 = item.Split("|", StringSplitOptions.TrimEntries);
            var outputs = s1[1].Split(" ");

            res += outputs.Count(o => (o.Length >= 2 && o.Length <= 4) || o.Length == 7);
        }
        return res;
    }
}