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
            return Enumerable.Range(0, rows).SelectMany(y => Enumerable.Range(0, columns).Select(x => (x, y))).ToList();
        }
        public static bool IsInGrid<T>((int x, int y) position, List<List<T>> grid)
        {
            return position.x >= 0 && position.y >= 0 && position.x < grid[0].Count && position.y < grid.Count;
        }
        public static (int xOff, int yOff)[] GetSquare4DirectionOffsets()
        {
            return [(0,-1),(1, 0),(0,1),(-1,0),];
        }
        public static (int xOff, int yOff)[] GetDiagonal4DirectionOffsets()
        {
            return [(-1,-1),(-1,1),(1,-1),(1, 1)];
        }
    }
}
