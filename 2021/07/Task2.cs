using CommonLib;
using CommonLib.Solvers;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var crabs = input[0].Split(",").Select(p => int.Parse(p));

        var minPos = crabs.Min();
        var maxPos = crabs.Max();

        var fuelToPos = new Dictionary<int, int>();
        for (var i = minPos; i <= maxPos; i++)
        {
            var fuelConsumption = 0;
            foreach (var crab in crabs)
            {
                var crabFuelCost = int.Abs(i - crab);
                var triangularNumber = crabFuelCost * (crabFuelCost + 1) / 2;
                fuelConsumption += triangularNumber;
            }
            fuelToPos.Add(i, fuelConsumption);
        }

        return fuelToPos.Values.Min();
    }
}