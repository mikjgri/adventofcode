using System.Net.Security;

public class Task2(string[] input)
{
    public void Solve(){

        var reports = input.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();

        var pro2 = reports.Where(report => report.Select((report, index) => (report, index)).Any(x =>
        {
            var clone = report.ToList().Skip(x.index).ToList();
            return IsSafe(clone);
        }));
        
        
        var pro = reports.Where(report =>
        {
            for (var i = 0; i < report.Count; i++)
            {
                var clonedReport = report.ToList();
                clonedReport.RemoveAt(i);
                if (IsSafe(clonedReport))
                {
                    return true;
                }
            }

            return false;
        });
        var pro3 = reports.Where(report =>
        {
            return report.Any(level =>
            {
                var clonedReport = report.ToList();
                clonedReport.RemoveAt(report.IndexOf(level));
                return IsSafe(clonedReport);
            });
        });
        Console.WriteLine(pro2.Count());
    }
    
    bool IsSafe(List<int> levels)
    {
        return IsAllIncreasingOrDecreasing(levels) && AdjecentWithinRange(levels);
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