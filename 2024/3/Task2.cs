using CommonLib;
using System.Text.RegularExpressions;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var matches = Regex.Matches(string.Concat(input), @"mul\((\d+),(\d+)\)|(?<go>do\(\))|(?<stop>don't\(\))");
        var go = true;
        Console.WriteLine(matches.Sum(match =>
        {
            if (match.Groups["go"].Success || match.Groups["stop"].Success)
            {
                go = match.Groups["go"].Success;
                return 0;
            }
            return go ? int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value) : 0;
        }));
    }
}