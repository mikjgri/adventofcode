using CommonLib;
using CommonLib.Solvers;
using System.Text.RegularExpressions;

public class Task2(string[] input) : BaseTask()
{
    const int MaxRisk = 3400;
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
        var velocityHit = new List<(int xVel, int yVel)>();

        for (int xVel = 0; xVel <= xRange.end; xVel++)
        {
            for (int yVel = yRange.start; yVel <= 100; yVel++)
            {
                var probePos = submarinePos;
                var simulatedXVel = xVel;
                var simulatedYVel = yVel;


                while (probePos.x <= xRange.end && probePos.y >= yRange.start)
                {
                    probePos.x += simulatedXVel;
                    probePos.y += simulatedYVel;

                    if (probePos.x >= xRange.start && probePos.x <= xRange.end && probePos.y >= yRange.start && probePos.y <= yRange.end) //hit
                    {
                        velocityHit.Add((xVel, yVel));
                        break;
                    }

                    if (simulatedXVel > 0) simulatedXVel--;
                    simulatedYVel--;
                }
            }
        }

        return velocityHit.Count;
    }
}