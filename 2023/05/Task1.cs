public class Task1(string[] input)
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
    public void Solve()
    {
        var maps = GetMaps();

        var lowestMap = double.MaxValue;

        var seeds = input[0].Split(":")[1].Trim().Split(" ").Select(s => double.Parse(s));
        foreach (var seed in seeds)
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
        Console.WriteLine(lowestMap);
    }
}