public class Task2
{
    private string[] _input;
    public Task2(string[] input)
    {
        _input = input;
    }
    private List<(string Source, string Target, List<(double source, double target, double range)> MapTable)> GetMaps()
    {
        var mapLines = _input[2..];
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
    public void Solve()
    {
        var maps = GetMaps();

        var lowestMap = double.MaxValue;

        var seedSplit = _input[0].Split(":")[1].Trim().Split(" ");

        var seedRanges = new List<(double start, double range)>();

        for (var i= 0; i < seedSplit.Length; i+=2)
        {
            seedRanges.Add((double.Parse(seedSplit[i]), double.Parse(seedSplit[i + 1])));
        }

        foreach (var seedRange in seedRanges)
        {
            for(var seed = seedRange.start; seed < seedRange.start+seedRange.range; seed++)
            {
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
                if (number < lowestMap) lowestMap = number;
            }
        }
        Console.WriteLine(lowestMap);
    }
}