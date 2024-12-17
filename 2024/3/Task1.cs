using CommonLib;
using System.Text.RegularExpressions;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        Console.WriteLine(input.Sum(line => Regex.Matches(line, @"mul\((\d+),(\d+)\)").Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))));
    }
}