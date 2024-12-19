using CommonLib;

public class Task2_caching(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var towelPatterns = input[0].Split(",").Select(x => x.Trim()).ToList();
        var towelDesigns = input[2 ..input.Length].ToList();

        var cache = new Dictionary<string, long>();
        Console.WriteLine(towelDesigns.Sum(isDesignPossible));
        long isDesignPossible(string remainingDesign)
        {
            if (string.IsNullOrEmpty(remainingDesign)) return 1;
            if (cache.TryGetValue(remainingDesign, out var res)) return res;
            var matchingPatterns = towelPatterns.Where(tp => remainingDesign.StartsWith(tp));
            long subres = matchingPatterns.Sum(mp => isDesignPossible(remainingDesign[mp.Length..]));
            cache.Add(remainingDesign, subres);
            return subres;
        }
    }
}