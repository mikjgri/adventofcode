namespace CommonLib;

public static class GridTools
{
    public static List<List<T>> InitializeEmptyGrid<T>(int columns, int rows)
    {
        return Enumerable.Range(0, columns).Select(c => Enumerable.Range(0, rows).Select(r => default(T)).ToList()).ToList();
    }
    public static List<List<T>> InitializeGridList<T>(int columns, int rows, T value)
    {
        return Enumerable.Range(0, rows).Select(c => Enumerable.Range(0, columns).Select(r => value).ToList()).ToList();
    }
    public static T[][] InitializeGridArray<T>(int columns, int rows, T value)
    {
        return Enumerable.Range(0, rows).Select(c => Enumerable.Range(0, columns).Select(r => value).ToArray()).ToArray();
    }
    public static List<(int x, int y)> GenerateCoordinates(int columns, int rows)
    {
        return Enumerable.Range(0, rows).SelectMany(y => Enumerable.Range(0, columns).Select(x => (x, y))).ToList();
    }
    public static (int x, int y) Offset((int x, int y) position, (int x, int y) offset)
    {
        return (position.x + offset.x, position.y + offset.y);
    }
    public static bool IsInGrid<T>((int x, int y) position, List<List<T>> grid)
    {
        return position.x >= 0 && position.y >= 0 && position.x < grid[0].Count && position.y < grid.Count;
    }
    public static int GetManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
    {
        return Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
    }
    public static (int xOff, int yOff)[] GetSquare4DirectionOffsets()
    {
        return [(0,-1),(1, 0),(0,1),(-1,0),];
    }
    public static (int xOff, int yOff)[] GetDiagonal4DirectionOffsets()
    {
        return [(-1,-1),(-1,1),(1,-1),(1, 1)];
    }
    public static (int xOff, int yOff)[] Get8DirectionOffsets()
    {
        return
        [
            (0, -1),   // Up
            (1, 0),     // Right
            (0, 1),     // Down
            (-1, 0),    // Left
            (-1, -1),   // Up-left
            (-1, 1),    // Down-left
            (1, -1),    // Up-right
            (1, 1)      // Down-right
        ];
    }
}
