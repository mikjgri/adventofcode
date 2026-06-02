using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<(int index, int item)> numbers = [.. input.Select((item, index) => (index, int.Parse(item)))];
        return numbers.Count(elem => elem.index != 0 && elem.item > numbers[elem.index - 1].item);
    }
}