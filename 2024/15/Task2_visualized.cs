using CommonLib;

public class Task2_visualized(string[] input) : BaseTask(input)
{
    protected override void Solve()
    {
        var grid = input[..input.ToList().IndexOf(string.Empty)].Select(line => string.Join(string.Empty, line.Select(c =>
        {
            return c switch
            {
                '.' => "..",
                '#' => "##",
                'O' => "[]",
                '@' => "@.",
                _ => throw new ArgumentException()
            };
        }))).CreateGrid();

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
        var boxes = coordinates.Where(c => grid[c.y][c.x] == '[').Select(c => new Box(c)).ToList();

        var robot = coordinates.First(c => grid[c.y][c.x] == '@');
        foreach (var movement in movements)
        {
            var robotAllowedToMove = false;
            grid.Visualize();
            (int x, int y) movedPosition = (robot.x + movement.xOff, robot.y + movement.yOff);
            var robotGridVal = grid[movedPosition.y][movedPosition.x];

            var boxAtPos = boxes.FirstOrDefault(box => box.Positions.Any(bPos => bPos == movedPosition));
            if (robotGridVal == '#')
            {
                continue;
            }
            if (robotGridVal == '.')
            {
                robotAllowedToMove = true;
            }
            else if (boxAtPos != null)
            {
                var movedBoxes = new List<Box>();
                var boxesCouldMove = MoveBoxes(boxAtPos, movedBoxes);
                if (boxesCouldMove)
                {
                    robotAllowedToMove = true;

                    foreach(var box in movedBoxes)
                    {
                        foreach (var prevPos in box.PrevPositions)
                        {
                            grid[prevPos.y][prevPos.x] = '.';
                        }
                    }
                    foreach (var box in movedBoxes)
                    {
                        grid[box.Positions.First().y][box.Positions.First().x] = '[';
                        grid[box.Positions.Last().y][box.Positions.Last().x] = ']';
                        box.Commit();
                    }
                }
                else
                {
                    movedBoxes.ForEach(b => b.Revert());
                }
            }
            if (robotAllowedToMove)
            {
                grid[robot.y][robot.x] = '.';
                grid[movedPosition.y][movedPosition.x] = '@';
                robot = movedPosition;
            }
            bool MoveBoxes(Box box, List<Box> alteredBoxes)
            {
                box.SetPos((box.Positions.First().x + movement.xOff, box.Positions.First().y + movement.yOff));
                alteredBoxes.Add(box);
                if (box.Positions.Any(pos => grid[pos.y][pos.x] == '#'))
                {
                    return false;
                }
                var hitBoxes = boxes.Where(bb => bb != box && box.Positions.Any(bp => bb.Positions.Contains(bp)));
                if (box == null)
                {
                    return true;
                }
                //else hit another box
                return hitBoxes.All(bb => MoveBoxes(bb, alteredBoxes));
            }
        }
        grid.Visualize();
        Console.WriteLine(coordinates.Where(c => boxes.Any(b => b.Positions.First() == c)).Sum(c => c.y * 100 + c.x));

    }
    class Box
    {
        public List<(int x, int y)> Positions;
        public List<(int x, int y)> PrevPositions;

        public Box((int x, int y) FirstPos)
        {
            Positions = [FirstPos, (FirstPos.x + 1, FirstPos.y)];
            PrevPositions = [.. Positions];
        }
        public void SetPos((int x, int y) pos)
        {
            Positions = [pos, (pos.x + 1, pos.y)];
        }
        public void Revert()
        {
            Positions = [.. PrevPositions];
        }
        public void Commit()
        {
            PrevPositions = [.. Positions];
        }
    }
}