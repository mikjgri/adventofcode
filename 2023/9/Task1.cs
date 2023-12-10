public class Task1(string[] input)
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
                iterations[i].Add(iterations[i].Last() + (i < iterations.Count - 1 ? iterations[i + 1].Last() : 0));
            }
            return iterations.First().Last();
        });
        Console.WriteLine(result);
    }
}