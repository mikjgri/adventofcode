using CommonLib;
using System.Text.RegularExpressions;

public class Task2(string[] input) : BaseTask()
{
    private readonly (int width, int height) gridSize = (101, 103);
    protected override void Solve()
    {
        var robots = input.Select(line =>
        {
            var numbers = Regex.Matches(line, "-*\\d+").Select(m => int.Parse(m.Value)).ToList();
            return new Robot()
            {
                Position = (numbers[0], numbers[1]),
                Velocity = (numbers[2], numbers[3])
            };
        }).ToList();

        var coordinates = GridTools.GenerateCoordinates(gridSize.width, gridSize.height);

        var i = 0;
        while(true)
        {
            i++;
            foreach (var robot in robots)
            {
                robot.Position = (Mod(robot.Position.x + robot.Velocity.x, gridSize.width), Mod(robot.Position.y + robot.Velocity.y, gridSize.height));
            }

            foreach(var robot in robots) //look for 10+ continous items vertically 
            {
                (int, int) GetNextDown(Robot r)
                {
                    return (r.Position.x, r.Position.y + 1);
                }
                var nextDown = GetNextDown(robot);
                var continousDown = 0;
                while (true)
                {
                    var nextRobot = robots.FirstOrDefault(r => r.Position == nextDown);
                    if (nextRobot == null)
                    {
                        break;
                    }
                    continousDown++;
                    nextDown = GetNextDown(nextRobot);
                }
                if (continousDown > 10)
                {
                    Visualize(robots);
                    Console.WriteLine(i);
                    return;
                }
            }
        }
    }
    class Robot
    {
        public (int x, int y) Position;
        public (int x, int y) Velocity;
    }
    private void Visualize(List<Robot> robots)
    {
        var testGrid = GridTools.InitializeGrid(gridSize.width, gridSize.height, ".");
        foreach (var robot in robots)
        {
            testGrid[robot.Position.y][robot.Position.x] = "#";
        }
        testGrid.Visualize();
    }
    //https://github.com/dotnet/csharplang/discussions/4744
    static int Mod(int a, int b)
    {
        int c = a % b;
        if ((c < 0 && b > 0) || (c > 0 && b < 0))
        {
            c += b;
        }
        return c;
    }
}