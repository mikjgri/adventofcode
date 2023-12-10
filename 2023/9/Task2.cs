public class Task2(string[] input)
{
    private List<List<int>> GetHistories()
    {
        return input.Select(line => line.Split(' ').Select(number => int.Parse(number)).ToList()).ToList();
    }
    public void Solve()
    {
        var histories = GetHistories();

        var result = histories.Sum(history =>
        {
            var iterations = new List<List<int>> { new(history) };
            while (!iterations.Last().All(x => x == 0))
            {
                iterations.Add(Enumerable.Range(1, iterations.Last().Count - 1).Select(i => iterations.Last()[i] - iterations.Last()[i - 1]).ToList());
            }
            for (int i = iterations.Count - 1; i >= 0; i--)
            {
                iterations[i].Insert(0, iterations[i][0] - (i < iterations.Count - 1 ? iterations[i + 1][0] : 0));
            }
            return iterations.First().First();
        });
        Console.WriteLine(result);
    }
}