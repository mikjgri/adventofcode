using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var grid = input.CreateGrid();

        var directionOffsets = GridTools.Get8DirectionOffsets();
        var allCoordinates = GridTools.GenerateCoordinates(grid[0].Count, input.Length);

        var accessableToiletRolls = allCoordinates.Where(coord => grid[coord.y][coord.x] == '@').Count(coord =>
        {
            return directionOffsets.Count(offset =>
            {
                var adjecentCoord = GridTools.Offset(coord, offset);
                if (!GridTools.IsInGrid(adjecentCoord, grid)) return false;
                return grid[adjecentCoord.y][adjecentCoord.x] == '@';
            }) < 4;
        });
        return accessableToiletRolls;
    }
}