using CommonLib;
using System.Text.RegularExpressions;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var towelPatterns = input[0].Split(",").Select(x => x.Trim()).ToList();
        var towelDesigns = input[2 ..input.Length];

        Console.WriteLine(towelDesigns.Count(isDesignPossible));

        bool isDesignPossible(string remainingDesign)
        {
            if (string.IsNullOrEmpty(remainingDesign)) return true;
            var matchingPatterns = towelPatterns.Where(tp => remainingDesign.StartsWith(tp));
            return matchingPatterns.Any(mp => isDesignPossible(remainingDesign[mp.Length..]));
        }
    }
}