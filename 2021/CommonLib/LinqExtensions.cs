namespace CommonLib;

public static class LinqExtensions
{
    public static IEnumerable<(T item, int index)> SelectWithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
}