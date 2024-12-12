using CommonLib;

public class Task2(string[] input)
{

    //DERP DERP, NO RUN
    public void Solve()
    {
        var grid = input.CreateGrid();
        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var squareDirections = GridTools.GetSquare4DirectionOffsets().ToList();
        var diagonalDirections = GridTools.GetDiagonal4DirectionOffsets();

        var handledCoordinates = new List<(int x, int y)>();

        var gardens = new List<FlowerGarden>();
        foreach (var coord in coordinates)
        {
            if (gardens.Any(garden => garden.Region.Contains(coord))) continue;
            gardens.Add(GetGarden(coord, grid[coord.y][coord.x], new FlowerGarden()));
        }
        //    var testGrid = Enumerable.Range(-2, 10)
        //.Select(x => Enumerable.Range(-2, 10)
        //    .Select(y => ".")
        //    .ToList())
        //.ToList();
        //    Console.WriteLine(gardens.Sum(garden =>
        //    {
        //        var start = garden.Region.First();
        //        var pos = start;
        //        var dirChange = 0;
        //        var prevDirIndex = -1;
        //        while (dirChange == 0 || pos != start)
        //        {
        //            Console.Clear();
        //            testGrid.Visualize();
        //            testGrid[pos.y][pos.x] = "*";
        //            var dir = directions.First(dir => garden.Region.Contains((pos.x + dir.xOff, pos.y + dir.yOff)));
        //            var dirIndex = directions.IndexOf(dir);
        //            if (dirIndex != prevDirIndex) dirChange++;
        //            pos = (pos.x + dir.xOff, pos.y + dir.yOff);
        //            var a = 123;
        //        }
        //        Console.WriteLine(dirChange);
        //        return garden.Region.Count * dirChange;
        //    }));

        Console.WriteLine(gardens[..1].Sum(garden =>
        {
            var multipliedPerimeterList = new List<(int x, int y)>();
            foreach(var peri in garden.Perimeter)
            {
                var numberOfRegionsUsing = squareDirections.Sum(dir => garden.Region.Count(reg => (reg.x + dir.xOff, reg.y + dir.yOff) == peri));
                multipliedPerimeterList.AddRange(Enumerable.Repeat(peri, numberOfRegionsUsing));
            }
            
            var start = multipliedPerimeterList.First();
            var remaining = multipliedPerimeterList[1..multipliedPerimeterList.Count];
            var pos = start;
            var dirChange = 1;
            for (var i = 0; i < multipliedPerimeterList.Count - 1; i++)
            {
                var nextNode = remaining.FirstOrDefault(rem => squareDirections.Any(dir => rem == (pos.x + dir.xOff, pos.y + dir.yOff)));
                if (nextNode == default) //not found in square
                {
                    dirChange++;
                    nextNode = remaining.FirstOrDefault(rem => diagonalDirections.Any(dir => rem == (pos.x + dir.xOff, pos.y + dir.yOff)));
                }
                remaining = remaining.Where(rem => rem != nextNode).ToList();
                pos = nextNode;
            }
            Console.WriteLine(dirChange);
            return garden.Region.Count * dirChange;
        }));

        FlowerGarden GetGarden((int x, int y) pos, char plantType, FlowerGarden garden)
        {
            if (garden.Region.Contains(pos)) return garden;

            if (!GridTools.IsInGrid(pos, grid) || grid[pos.y][pos.x] != plantType)
            {
                garden.Perimeter.Add(pos);
            }
            else
            {
                garden.Region.Add(pos);
                squareDirections.ToList().ForEach(dir => GetGarden((pos.x + dir.xOff, pos.y + dir.yOff), plantType, new FlowerGarden() { Region = garden.Region, Perimeter = garden.Perimeter }));
            }
            return garden;
        }
    }
    class FlowerGarden
    {
        public List<(int x, int y)> Perimeter = [];
        public List<(int x, int y)> Region = [];
    }
}