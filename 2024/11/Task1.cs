public class Task1(string[] input)
{
    public void Solve()
    {
        var stones = input[0].Split(' ').Select(s => long.Parse(s)).ToList();

        for (var blink = 0; blink < 25; blink++)
        {
            for (int i = 0; i < stones.Count; i++)
            {
                var stone = stones[i];
                if (stone == 0)
                {
                    stones[i] = 1;
                }
                else if (long.IsEvenInteger(stone.ToString().Length))
                {
                    var middle = stone.ToString().Length / 2;
                    stones[i] = long.Parse(stone.ToString()[..middle]);
                    i++;
                    stones.Insert(i, long.Parse(stone.ToString()[middle..]));
                }
                else
                {
                    stones[i] = stone * 2024;
                }
            }
        }
        Console.WriteLine(stones.Count);
    }
}