using System.Net.Security;

public class Task1(string[] input)
{
    public void Solve(){

        var reports = input.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        Console.WriteLine(reports.Where(IsAllIncreasingOrDecreasing).Where(AdjecentWithinRange).Count());
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