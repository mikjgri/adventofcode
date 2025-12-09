using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        List<(int x, int y)> redTiles = [.. input
            .Select(line =>{
                var s = line.Split(",");
                return (int.Parse(s[0]), int.Parse(s[1]));
            }
        )];
        var redPairs = redTiles
            .SelectMany((t1, i) => redTiles
                .Skip(i + 1)
                .Select(t2 => (t1, t2)))
            .ToList();

        HashSet<(int x, int y)> greenTiles = [];

        //create hashset of green tiles
        foreach (var t1 in redTiles)
        {
            var t2 = redTiles[(redTiles.IndexOf(t1) + 1) % redTiles.Count];

            if (t1.x != t2.x)
            {
                addGreenTiles((Math.Min(t1.x, t2.x), t1.y), Math.Abs(t1.x - t2.x), 'x');
            }
            else
            {
                addGreenTiles((t1.x, Math.Min(t1.y, t2.y)), Math.Abs(t1.y - t2.y), 'y');
            }
        }
        void addGreenTiles((int x, int y) start, int count, char axis)
        {
            for (var i = 1; i < count; i++)
            {
                greenTiles.Add((axis == 'x' ? start.x + i : start.x, axis == 'y' ? start.y + i : start.y));
            }
        }

        //create super awesome optimization dictionaries to quickly check if position is within boundries
        var rangeYatX = new Dictionary<int, (int min, int max)>();
        var rangeXatY = new Dictionary<int, (int min, int max)>();
        foreach (var (x, y) in redTiles.Concat(greenTiles))
        {
            updateDictionary(x, y, rangeYatX);
            updateDictionary(y, x, rangeXatY);
        }
        void updateDictionary(int key, int value, Dictionary<int, (int min, int max)> dict)
        {
            if (!dict.TryGetValue(key, out var dictVal))
            {
                dict.Add(key, (value, value));
            }
            else
            {
                if (dictVal.min > value) dictVal.min = value;
                if (dictVal.max < value) dictVal.max = value;
                dict[key] = dictVal;
            }
        }

        //add size to all pairs and order by that
        var orderedRedPairsWithSize = redPairs.Select(rP => (rP.t1, rP.t2, (Math.Abs((long)rP.t1.x - rP.t2.x) + 1) * (Math.Abs((long)rP.t1.y - rP.t2.y) + 1))).OrderByDescending(rP => rP.Item3).ToList();

        //find first that has all rectangle positions within boundries
        var max = orderedRedPairsWithSize.First((rP) =>
        {
            Console.WriteLine($"{orderedRedPairsWithSize.IndexOf(rP)}/{orderedRedPairsWithSize.Count}");
            var minX = Math.Min(rP.t1.x, rP.t2.x);
            var maxX = Math.Max(rP.t1.x, rP.t2.x);
            var minY = Math.Min(rP.t1.y, rP.t2.y);
            var maxY = Math.Max(rP.t1.y, rP.t2.y);

            //check boundry first (quicker)
            for (var x = minX; x < maxX; x++)
            {
                if (!isWithinBoundry((x, minY))) return false;
                if (!isWithinBoundry((x, maxY))) return false;
            }
            for (var y = minY; y < maxY; y++)
            {
                if (!isWithinBoundry((minX, y))) return false;
                if (!isWithinBoundry((maxX, y))) return false;
            }

            //check content (zzz)
            for (var x = minX+1; x < maxX-1; x++)
            {
                for (var y = minY+1; y < maxY-1; y++)
                {
                    if (!isWithinBoundry((x, y))) return false;
                }
            }
            return true;
        });
        bool isWithinBoundry((int x, int y) p)
        {
            var rangeYatXValue = rangeYatX[p.x];
            var rangeXatYValue = rangeXatY[p.y];
            //pro tip. to cache your results you need to invest your bank account savings in memory sticks 🙃.. Soo... dont.
            return p.x >= rangeXatYValue.min && p.x <= rangeXatYValue.max &&
                p.y >= rangeYatXValue.min && p.y <= rangeYatXValue.max;

        }
        Console.WriteLine(max.Item3);
    }
}
