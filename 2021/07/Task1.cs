using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var crabs = input[0].Split(",").Select(p => int.Parse(p));

        var minPos = crabs.Min();
        var maxPos = crabs.Max();

        var fuelToPos = new Dictionary<int, int>();
        for (var i = minPos; i<=maxPos; i++)
        {
            var fuelConsumption = 0;
            foreach (var crab in crabs)
            {
                fuelConsumption += int.Abs(i - crab);
            }
            fuelToPos.Add(i, fuelConsumption);
        }

        return fuelToPos.Values.Min();
    }
}