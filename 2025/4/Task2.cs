using CommonLib;

public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {

        var directionOffsets = GridTools.Get8DirectionOffsets();
        var allCoordinates = GridTools.GenerateCoordinates(input[0].Length, input.Length);

        var totalRemoved = 0;

        var grid = input.CreateGrid();
        var accessableToiletRolls = new List<(int x, int y)>();
        while (totalRemoved == 0 || accessableToiletRolls.Count > 0)
        {
            accessableToiletRolls = [.. allCoordinates.Where(coord => grid[coord.y][coord.x] == '@').Where(coord =>
            {
                return directionOffsets.Count(offset =>
                {
                    var adjecentCoord = GridTools.Offset(coord, offset);
                    if (!GridTools.IsInGrid(adjecentCoord, grid)) return false;
                    return grid[adjecentCoord.y][adjecentCoord.x] == '@';
                }) < 4;
            })];
            totalRemoved += accessableToiletRolls.Count;
            accessableToiletRolls.ForEach(coord => grid[coord.y][coord.x] = '.');
        }

        Console.WriteLine(totalRemoved);
    }
}