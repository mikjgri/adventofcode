namespace CommonLib;

public static class GridExtensions
{
    public static void Visualize<T>(this IEnumerable<IEnumerable<T>> grid)
    {
        grid.ToList().ForEach(columns => { columns.ToList().ForEach(item => Console.Write(item.ToString())); Console.Write(Environment.NewLine); });
    }
    public static List<List<char>> CreateGrid(this IEnumerable<string> input)
    {
        return [.. input.Select(line => line.Select(c => c).ToList())];
    }
}
