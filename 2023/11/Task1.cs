using System.ComponentModel.DataAnnotations;

public class Task1(string[] input)
{
    private List<List<string>> GetExpandedStarMap()
    {
        var map = input.Select(line => line.Select(c => c.ToString()).ToList()).ToList();

        for (var line = 0; line < map[0].Count; line++)
        {
            if (map.All(row => row[line] == "."))
            {
                foreach (var row in map)
                {
                    row.Insert(line, ".");
                }
                line++;
            }
        }
        for (var row = 0; row < map.Count; row++)
        {
            if (map[row].All(line => line == "."))
            {
                map.Insert(row, [.. map[row]]);
                row++;
            }
        }
        return map;
    }
    private List<List<(int x, int y)>> GetGalaxyPairs(List<List<string>> starMap)
    {
        var galaxies = new List<(int, int)>();
        for (var y = 0; y < starMap.Count; y++)
        {
            for (var x = 0; x < starMap[0].Count; x++)
            {
                if (starMap[y][x] == "#")
                {
                    galaxies.Add((x, y));
                }
            }
        }
        var ret = new List<List<(int, int)>>();
        foreach (var firstGalaxy in galaxies)
        {
            foreach (var secondGalaxy in galaxies)
            {
                if (firstGalaxy != secondGalaxy && !ret.Any(pair => pair.Contains(firstGalaxy) && pair.Contains(secondGalaxy)))
                {
                    ret.Add([firstGalaxy, secondGalaxy]);
                }
            }
        }
        return ret;
    }
    public void Solve()
    {
        var starMap = GetExpandedStarMap();
        var galaxyPairs = GetGalaxyPairs(starMap);

        var sum = galaxyPairs.Sum(pair => Math.Abs(pair[0].x - pair[1].x) + Math.Abs(pair[0].y - pair[1].y));
        Console.WriteLine(sum);
    }
}