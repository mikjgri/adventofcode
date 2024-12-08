namespace CommonLib
{
    public static class GridTools
    {
        public static List<List<T>> InitializeEmptyGrid<T>(int columns, int rows)
        {
            return Enumerable.Range(0, columns).Select(c => Enumerable.Range(0, rows).Select(r => default(T)).ToList()).ToList();
        }
        public static List<List<T>> InitializeGrid<T>(int columns, int rows, T value)
        {
            return Enumerable.Range(0, columns).Select(c => Enumerable.Range(0, rows).Select(r => value).ToList()).ToList();
        }
        public static List<(int x, int y)> GenerateCoordinates(int columns, int rows)
        {
            return Enumerable.Range(0, columns).SelectMany(y => Enumerable.Range(0, rows).Select(x => (x, y))).ToList();
        }
        public static bool IsInGrid<T>((int x, int y) position, List<List<T>> grid)
        {
            return position.x >= 0 && position.y >= 0 && position.x < grid[0].Count && position.y < grid.Count;
        }
    }
}
