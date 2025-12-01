using Spectre.Console;

namespace CommonLib;

public static class FancyGridVisualizerExtensions
{
    public static void FancyVisualize<T>(this IEnumerable<IEnumerable<T>> grid)
    {
        var palette = new[]
        {
                Color.Red, Color.Green, Color.Blue,
                Color.Yellow, Color.Purple, Color.Cyan1,
                Color.White, Color.CornflowerBlue, Color.Fuchsia,
                Color.Orange1, Color.Pink1, Color.Aqua,
                Color.Chartreuse1, Color.Gold3, Color.SlateBlue1
            };
        var colorMap = new Dictionary<string, Color>();

        var vGrid = new Grid();
        foreach (var _ in grid.ToArray()[0])
        {
            vGrid.AddColumn(new GridColumn().Padding(0, 0, 0, 0));
        }

        foreach (var row in grid)
        {
            vGrid.AddRow(row.Select(r =>
            {
                var val = r.ToString();
                var hash = val.GetHashCode();
                if (hash < 0) hash = -hash;
                var color = palette[hash % palette.Length];
                return new Text(r.ToString(), new Style(color));
            }
            ).ToArray());
        }
        AnsiConsole.Write(vGrid);
    }
}
