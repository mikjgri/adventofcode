using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var diskMap = input[0].Select(c => int.Parse(c.ToString())).ToList();

        var sanitized = diskMap.Select((digit, i) =>
        {
            var isFiles = i % 2 == 0;
            return Enumerable.Range(0, digit).Select(c => isFiles
                ? (i - (i / 2)).ToString()
                : ".");
        }).SelectMany(dm => dm).ToList();

        long checksum = 0;
        for (int i = 0; i < sanitized.Count; i++)
        {
            if (sanitized[i] == ".")
            {
                var lastNumberIndex = sanitized.FindLastIndex(num => !Equals(num, "."));
                var lastValue = sanitized[lastNumberIndex];
                if (lastNumberIndex < i) break;
                sanitized[i] = sanitized[lastNumberIndex];
                sanitized.RemoveAt(lastNumberIndex);
            }
            checksum += i * long.Parse(sanitized[i]);
        }
        Console.WriteLine(checksum);
    }
}