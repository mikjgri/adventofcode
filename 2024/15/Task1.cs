using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var grid = input[..input.ToList().IndexOf(string.Empty)].CreateGrid();
        List<(int xOff, int yOff)> movements = string.Join(string.Empty, input[grid.Count..]).Select(c =>
        {
            return c switch
            {
                '<' => (-1, 0),
                '>' => (1, 0),
                '^' => (0, -1),
                'v' => (0, 1),
                _ => throw new ArgumentException(),
            };
        }).ToList();

        var coordinates = GridTools.GenerateCoordinates(grid[0].Count, grid.Count);
        var robot = coordinates.First(c => grid[c.y][c.x] == '@');
        foreach (var movement in movements)
        {
            var robotAllowedToMove = false;
            (int x, int y) movedPosition = (robot.x + movement.xOff, robot.y + movement.yOff);
            var robotGridVal = grid[movedPosition.y][movedPosition.x];
            if (robotGridVal == '#')
            {
                continue;
            }
            if (robotGridVal == '.')
            {
                robotAllowedToMove = true;
            }
            else if (robotGridVal == 'O')
            {
                var newBoxes = NewBoxPositions(movedPosition, new List<(int x, int y)>());
                if (newBoxes.Any())
                {
                    robotAllowedToMove = true;
                    foreach (var box in newBoxes)
                    {
                        grid[box.y][box.x] = 'O';
                    }
                }
            }
            if (robotAllowedToMove)
            {
                grid[robot.y][robot.x] = '.';
                grid[movedPosition.y][movedPosition.x] = '@';
                robot = movedPosition;
            }
            List<(int x, int y)> NewBoxPositions((int x, int y) box, List<(int x, int y)> boxes)
            {
                box = (box.x + movement.xOff, box.y + movement.yOff);
                var gridVal = grid[box.y][box.x];
                if (gridVal == '#')
                {
                    return [];
                }
                boxes.Add(box);
                if (gridVal == '.')
                {
                    return boxes;
                }
                //else hit another box
                return NewBoxPositions(box, boxes);
            }
        }
        Console.WriteLine(coordinates.Where(c => grid[c.y][c.x] == 'O').Sum(c => c.y * 100 + c.x));
        
    }
}