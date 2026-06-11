using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var lanternFish = input[0].Split(",").Select(p => int.Parse(p)).ToList();

        for (var i = 0; i < 80; i++)
        {
            var fishiesAtTheStartOfTheDay = lanternFish.Count;
            for (var j = 0; j < fishiesAtTheStartOfTheDay; j++)
            {
                lanternFish[j]--;
                if (lanternFish[j] < 0)
                {
                    lanternFish[j] = 6;
                    lanternFish.Add(8);
                }
            }
        }
        return lanternFish.Count;
    }
}