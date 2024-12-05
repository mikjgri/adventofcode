using System.Diagnostics.Metrics;

public class Task2(string[] input)
{
    public void Solve()
    {
        List<(int before, int after)> rules = input.TakeWhile(line => !string.IsNullOrEmpty(line)).Select(line => { var s = line.Split("|"); return (int.Parse(s[0]), int.Parse(s[1])); }).ToList();
        var updates = input[(rules.Count + 1)..].Select(line => line.Split(",").Select(page => int.Parse(page)).ToList()).ToList();

        var incorrectlyOrderedPagesFixed = updates
            .Where(pages => pages.Any(page => rules
            .Where(rule => rule.after == page).Any(rule => pages
            .Any(pageToCheck => pageToCheck != page && pageToCheck == rule.before && pages.IndexOf(pageToCheck) > pages.IndexOf(page)))))
            .Select(fixViolations);

        Console.WriteLine(incorrectlyOrderedPagesFixed.Sum(pages => pages[pages.Count / 2]));

        List<int> fixViolations(List<int> pages)
        {
            foreach (var (before, after) in rules)
            {
                var p1 = pages.IndexOf(before);
                var p2 = pages.IndexOf(after);
                if (p1 > -1 && p2 > -1 && p1 > p2)
                {
                    var v1 = pages[p1];
                    pages.RemoveAt(p1);
                    pages.Insert(p2, v1);
                    return fixViolations(pages);
                }
            }
            return pages;
        }
    }
}