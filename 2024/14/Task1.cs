using CommonLib;
using System.Text.RegularExpressions;

public class Task1(string[] input)
{
    private readonly (int width, int height) gridSize = (101, 103);
    private readonly int iterations = 100;
    public void Solve()
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

        for (var i = 0; i < iterations; i++)
        {
            foreach (var robot in robots)
            {
                robot.Position = (Mod(robot.Position.x + robot.Velocity.x, gridSize.width), Mod(robot.Position.y + robot.Velocity.y, gridSize.height));
            }
        }

        var halfWidth = gridSize.width / 2;
        var halfHeight = gridSize.height / 2;
        Console.WriteLine(
            countRobots((0, 0), (halfWidth-1, halfHeight-1)) * //upper left
            countRobots((halfWidth+1,0), (gridSize.width, halfHeight-1)) * //upper right
            countRobots((0, halfHeight+1), (halfWidth -1, gridSize.height)) * //lower left
            countRobots((halfWidth + 1, halfHeight+1), (gridSize.width, gridSize.height)) //lower right
            );
        int countRobots((int x, int y) min, (int x, int y) max)
        {
            return robots.Count(robot => robot.Position.x >= min.x && robot.Position.x <= max.x && robot.Position.y >= min.y && robot.Position.y <= max.y);
        }
    }
    class Robot
    {
        public (int x, int y) Position;
        public (int x, int y) Velocity;
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