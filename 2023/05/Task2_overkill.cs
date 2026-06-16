using System.Collections.Concurrent;

public class Task2_overkill(string[] input)
{
    private List<(string Source, string Target, List<(double source, double target, double range)> MapTable)> GetMaps()
    {
        var mapLines = input[2..];
        var maps = new List<(string Source, string Target, List<(double source, double target, double range)> MapTable)>();

        foreach (var line in mapLines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            if (line.Contains("map:"))
            {
                var mapSplit = line.Split(" ")[0].Split("-");
                maps.Add((mapSplit[0], mapSplit[2], new List<(double source, double target, double range)>()));
                continue;
            }
            var rangeSplit = line.Split(" ");

            maps.Last().MapTable.Add((double.Parse(rangeSplit[1]), double.Parse(rangeSplit[0]), double.Parse(rangeSplit[2])));
        }
        return maps;
    }

    private ConcurrentDictionary<int, double> ThreadPercent = new ConcurrentDictionary<int, double>();
    private async Task LoopStatus()
    {
        await Task.Delay(1000);
        Console.Clear();
        Console.WriteLine("Multi threaded seed location calculation");
        foreach (var thread in ThreadPercent)
        {
            Console.WriteLine($"Thread: {thread.Key} {thread.Value}%");
        }
        Console.WriteLine($"{Environment.NewLine}{ThreadPercent.Count(item => item.Value == 100)}/{ThreadPercent.Count} threads done...");
        await LoopStatus();
    }
    public void Solve()
    {
        var maps = GetMaps();

        var lowestMap = double.MaxValue;

        var seedSplit = input[0].Split(":")[1].Trim().Split(" ");

        var seedRanges = new List<(double start, double range)>();

        for (var i= 0; i < seedSplit.Length; i+=2)
        {
            seedRanges.Add((double.Parse(seedSplit[i]), double.Parse(seedSplit[i + 1])));
        }
        var possiblyLowest = new ConcurrentBag<double>();

        var statusTask = LoopStatus();

        var myLock = new object();
        Parallel.ForEach(seedRanges, seedRange =>
        {
            for (var seed = seedRange.start; seed < seedRange.start + seedRange.range; seed++)
            {
                if ((seed - seedRange.start) % Math.Round(seedRange.range / 100) == 1)
                {
                    ThreadPercent.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (key, current) => Math.Floor(((seed - seedRange.start) * 100) / seedRange.range));
                }

                var source = "seed";
                var number = seed;
                while (source != "location")
                {
                    var map = maps.First(item => item.Source == source);
                    source = map.Target;

                    foreach (var table in map.MapTable)
                    {
                        if (number >= table.source && number <= table.source + table.range)
                        {
                            number = (number - table.source) + table.target;
                            break;
                        }
                    }
                }
                if (number < lowestMap)
                {
                    lock(myLock)
                    {
                        lowestMap = number;
                        possiblyLowest.Add(number);
                    }
                }
            }
            ThreadPercent.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (key, current) => 100);
        });
        statusTask.Dispose();
        Console.WriteLine($"Lowest location: {possiblyLowest.Min()}");
    }
}