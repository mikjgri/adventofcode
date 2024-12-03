public class Task2(string[] input)
{
    public void Solve(){

        var reports = input.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();

        Console.WriteLine(reports.Count(report => GetProblemDampened(report).Where(IsAllIncreasingOrDecreasing).Any(AdjecentWithinRange)));
    }
    
    List<List<int>> GetProblemDampened(List<int> report)
    {
        return report.Select((report, index) => (report, index)).Select(x => report.ToList().Where((item, index) => index != x.index).ToList()).ToList();
    }
    bool IsAllIncreasingOrDecreasing(List<int> levels)
    {
        var levelsWithIndex = levels.Select((level, index) => (level, index)).ToList();
        return levelsWithIndex.TrueForAll((item) => item.index == 0 || item.level > levels[item.index - 1])
               || levelsWithIndex.TrueForAll((item) => item.index == 0 || item.level < levels[item.index - 1]);
    }
    bool AdjecentWithinRange(List<int> levels)
    {
        return levels.Select((level, index) => (level, index)).ToList()
            .TrueForAll((item) => item.index == 0 || Math.Abs(levels[item.index - 1] - item.level) is >= 1 and <= 3);
    }
}