using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        List<(long start, long end)> freshRanges = [.. input.TakeWhile(x => !string.IsNullOrEmpty(x)).Select(r =>
        {
            var s = r.Split("-");
            return (long.Parse(s[0]), long.Parse(s[1]));
        })];
        var ingredients = input[(freshRanges.Count + 1)..].Select(x => long.Parse(x));

        Console.WriteLine(ingredients.Count(ingredient => freshRanges.Any(range => ingredient >= range.start && ingredient <= range.end)));
    }
}