using CommonLib;

public class Task2(string[] input)
{
    public void Solve()
    {
        var diskMap = input[0].Select(c => int.Parse(c.ToString())).ToList();

        var sanitized = diskMap.Select((digit, i) =>
        {
            var isFiles = i % 2 == 0;
            return Enumerable.Range(0, digit).Select(c => isFiles
                ? (i - (i / 2)).ToString()
                : ".").ToList();
        }).ToList();

        var max = sanitized.Count / 2;

        for (int id = max; id >= 0; id--)
        {
            var currentIndex = sanitized.FindIndex(item => item.Any() && item.First() == id.ToString());
            if (currentIndex == -1) continue;
            var currentValue = sanitized[currentIndex];
            var firstMatchingDotIndex = sanitized.FindIndex(item => item.Any() && currentValue.Count <= item.Count && item.First() == ".");
            if (firstMatchingDotIndex == -1 || firstMatchingDotIndex > currentIndex) continue;
            var firstDotValue = sanitized[firstMatchingDotIndex];

            sanitized[firstMatchingDotIndex] = currentValue;
            sanitized[currentIndex] = Enumerable.Repeat(".", currentValue.Count).Select(c => c.ToString()).ToList();
            if (currentValue.Count != firstDotValue.Count)
            {
                sanitized.Insert(firstMatchingDotIndex + 1, Enumerable.Repeat(".", firstDotValue.Count - currentValue.Count).Select(c => c.ToString()).ToList());
            }
            //Console.WriteLine(string.Join(string.Empty, sanitized.SelectMany(s => s)));
        }
        long checksum = 0;
        checksum = sanitized.SelectMany(s => s).SelectWithIndex().Where(c => !Equals(c.item, ".")).Sum(n => long.Parse(n.item) * n.index);
        Console.WriteLine(checksum);
    }
}