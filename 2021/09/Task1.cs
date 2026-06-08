using CommonLib;
using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var grid = input.Select(row => row.Select(col => int.Parse(col.ToString())).ToList()).ToList();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);

        var offsets = GridTools.GetSquare4DirectionOffsets();

        return coordinates.Sum(coord =>
        {
            var gVal = grid[coord.y][coord.x];
            return offsets.All(offset =>
            {
                (int x, int y) oCoord = (coord.x + offset.xOff, coord.y + offset.yOff);

                return GridTools.IsInGrid(oCoord, grid) ? grid[oCoord.y][oCoord.x] > gVal : true;
            }) ? gVal+1 : 0;
        });
    }
}