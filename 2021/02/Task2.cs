using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var x = 0;
        var z = 0;
        var aim = 0;
        foreach (var line in input)
        {
            var s = line.Split(" ");
            var direction = s[0];
            var value = int.Parse(s[1]);
            if (direction == "forward")
            {
                x += value;
                z += aim * value;
                continue;
            }
            if (direction == "down")
            {
                aim += value;
                continue;
            }
            aim -= value;
        }
        return x * z;
    }
}