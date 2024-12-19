using CommonLib;
using System.Collections.Concurrent;

public class Task2_bruteforce(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var towelPatterns = input[0].Split(",").Select(x => x.Trim()).ToList();
        var towelDesigns = input[2 ..input.Length].SelectWithIndex().ToList();

        var result = new ConcurrentBag<int>();
        Parallel.ForEach(towelDesigns, tD =>
        {
            var threadRes = isDesignPossible(tD.item);
            result.Add(threadRes);
            Console.WriteLine($"Done with: {tD.index}. Remaining items: {towelDesigns.Count - result.Count}");
        });

        Console.WriteLine(result.Sum());

        int isDesignPossible(string remainingDesign)
        {
            if (string.IsNullOrEmpty(remainingDesign)) return 1;
            var matchingPatterns = towelPatterns.Where(tp => remainingDesign.StartsWith(tp));
            return matchingPatterns.Sum(mp => isDesignPossible(remainingDesign[mp.Length..]));
        }
    }
}