using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        List<(int before, int after)> rules = input.TakeWhile(line => !string.IsNullOrEmpty(line)).Select(line => { var s = line.Split("|"); return (int.Parse(s[0]), int.Parse(s[1])); }).ToList();
        var updates = input[(rules.Count + 1)..].Select(line => line.Split(",").Select(page => int.Parse(page)).ToList()).ToList();

        var correctlyOrdered = updates
            .Where(pages => pages.All(page => rules
            .Where(rule => rule.before == page).All(rule => pages
            .All(pageToCheck => pageToCheck != rule.after || pages.IndexOf(pageToCheck) > pages.IndexOf(page)))));

        Console.WriteLine(correctlyOrdered.Sum(pages => pages[pages.Count / 2]));
    }
}