using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var x = 0;
        var z = 0;
        foreach (var line in input)
        {
            var s = line.Split(" ");
            var direction = s[0];
            var movement = int.Parse(s[1]);
            if (direction == "forward")
            {
                x += movement;
                continue;
            }
            if (direction == "down")
            {
                z += movement;
                continue;
            }
            z -= movement;
        }
        return x * z;
    }
}