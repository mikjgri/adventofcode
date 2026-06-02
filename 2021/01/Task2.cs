using CommonLib;
using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var numbers = input.Select(x => int.Parse(x)).ToList();
        var sums = numbers
            .Select((item, index) => index > numbers.Count - 3 ? -1 : numbers[index] + numbers[index + 1] + numbers[index + 2])
            .Where(x => x > -1)
            .SelectWithIndex()
            .ToList();


        return sums.Count(elem => elem.index != 0 && elem.item > sums[elem.index - 1].item);
    }
}