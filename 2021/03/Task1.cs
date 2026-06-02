using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var gammaRate = "";
        for (var i = 0; i < input[0].Length; i++)
        {
            var bits = input.Select(line => line[i]);
            gammaRate += bits.Count(b => b == '1') > input.Length / 2 ? "1" : "0";
        }
        var epsilonRate = string.Concat(gammaRate.Select(bit => bit == '1' ? '0' : '1'));
        return Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);
    }
}