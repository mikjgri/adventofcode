using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var lanternFishInput = input[0].Split(",").Select(p => int.Parse(p)).ToList();

        var lanternFish = new long[9];
        foreach (var fish in lanternFishInput)
        {
            lanternFish[fish]++;
        }

        for (var i = 0; i < 256; i++)
        {
            var originalDay = new long[9];
            lanternFish.CopyTo(originalDay);

            for (var j = 1; j < lanternFish.Length; j++)
            {
                lanternFish[j - 1] = originalDay[j];
            }
            lanternFish[8] = originalDay[0];
            lanternFish[6] += originalDay[0];
        }

        return lanternFish.Sum();
    }
}