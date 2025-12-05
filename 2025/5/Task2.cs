using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        List<(long start, long end)> ranges = [.. input.TakeWhile(x => !string.IsNullOrEmpty(x)).Select(r =>
        {
            var s = r.Split("-");
            return (long.Parse(s[0]), long.Parse(s[1]));
        }).Distinct()];

        var compressedRanges = true;
        while (compressedRanges)
        {
            compressedRanges = false;
            var rCopy = ranges.ToList();

            foreach (var range in ranges)
            {
                var otherRanges = ranges.Where(r => r != range).ToList();

                // complete overlap. remove
                var fullySwallowedBy = otherRanges.FirstOrDefault(r => r.start <= range.start && r.end >= range.end);
                if (fullySwallowedBy != default) 
                {
                    rCopy.Remove(range);
                    compressedRanges = true;
                    break;
                }

                //starts inside, ends outside. replace with single range
                var startsInsideEndsOutside = otherRanges.FirstOrDefault(r => r.start >= range.start && r.start <= range.end && r.end >= range.end);
                if (startsInsideEndsOutside != default)
                {
                    rCopy.Remove(range);
                    rCopy.Remove(startsInsideEndsOutside);
                    rCopy.Add((range.start, startsInsideEndsOutside.end));
                    compressedRanges = true;
                    break;
                }
            }
            ranges = [.. rCopy.Distinct()];
        }
        Console.WriteLine(ranges.Sum(r => r.end - r.start+1));
    }

}
