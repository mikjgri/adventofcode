using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var dialPosition = 50;
        var instructions = input.Select(line => int.Parse(line[1..]) * (line.First() == 'L' ? -1 : 1));

        var sum = instructions.Sum(instr =>
        {
            dialPosition = (dialPosition+instr).Mod(100);
            return dialPosition == 0 ? 1 : 0;
        });
        return sum;
    }
}