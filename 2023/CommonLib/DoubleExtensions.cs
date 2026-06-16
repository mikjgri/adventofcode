namespace CommonLib;

public static class DoubleExtensions
{
    public static IEnumerable<double> Range(this double start, double count)
    {
        for (int i = 0; i < count; i++)
            yield return start + i;
    }
}
