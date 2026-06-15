using CommonLib;
using CommonLib.Solvers;
using System.Text.RegularExpressions;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var s1 = input[0].Split(":", StringSplitOptions.TrimEntries);
        var s2 = s1[1].Split(",", StringSplitOptions.TrimEntries);
        (int start, int end) xRange = GetNumbers(s2[0]);
        (int start, int end) yRange = GetNumbers(s2[1]);

        (int x, int y) submarinePos = (0, 0);

        (int, int) GetNumbers(string str)
        {
            var numbers = Regex.Matches(str, @"-?\d+").Select(m => int.Parse(m.Value)).ToList();
            return (numbers[0], numbers[1]);
        }
        var bestHeight = int.MinValue;

        for (int xVel = 0; xVel < xRange.end; xVel++)
        {
            for (int yVel = 0; yVel < 100; yVel++)
            {
                var probePos = submarinePos;
                var simulatedXVel = xVel;
                var simulatedYVel = yVel;

                var probeMaxHeight = int.MinValue;

                while (probePos.x < xRange.end && probePos.y > yRange.end)
                {
                    probePos.x += simulatedXVel;
                    probePos.y += simulatedYVel;
                    if (probeMaxHeight < probePos.y) probeMaxHeight = probePos.y;

                    if (probePos.x >= xRange.start && probePos.x <= xRange.end && probePos.y >= yRange.start && probePos.y <= yRange.end) //hit
                    {
                        if (bestHeight < probeMaxHeight) bestHeight = probeMaxHeight;
                        break;
                    }

                    if (simulatedXVel > 0) simulatedXVel--;
                    simulatedYVel--;
                }
            }
        }

        return bestHeight;
    }
}