using Microsoft.Extensions.Caching.Memory;

public class Task2(string[] input)
{
    public void Solve()
    {
        var stones = input[0].Split(' ').Select(s => long.Parse(s)).ToList();

        var cache = new MemoryCache(new MemoryCacheOptions());

        long getStones(long stone, int blink)
        {
            var key = $"{stone}-{blink}";
            return cache.GetOrCreate(key, s =>
            {
                var stones = new List<long>() { stone };
                var stoneStr = stone.ToString();
                if (stone == 0)
                {
                    stones[0] = 1;
                }
                else if (long.IsEvenInteger(stoneStr.Length))
                {
                    var middle = stoneStr.Length / 2;
                    stones[0] = long.Parse(stoneStr[..middle]);
                    stones.Add(long.Parse(stoneStr[middle..]));
                }
                else
                {
                    stones[0] = stone * 2024;
                }
                long res = 0;
                if (blink >= 75)
                {
                    res = stones.Count;
                }
                else
                {
                    res = stones.Sum(x => getStones(x, blink + 1));
                }
                cache.Set(key, res);
                return res;
            });
        }

        Console.WriteLine(stones.Sum(s => getStones(s, 1)));
    }
}