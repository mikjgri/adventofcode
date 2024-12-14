using System.Text.RegularExpressions;

public class Task1(string[] input)
{
    public void Solve()
    {

        var machines = new List<Machine>();
        while (input.Length > 0)
        {
            var lines = input[..3];
            machines.Add(new Machine()
            {
                ButtonA = GetFirstTwoInts(lines[0]),
                ButtonB = GetFirstTwoInts(lines[1]),
                Prize = GetFirstTwoInts(lines[2])
            });
            input = input[(input.Length > lines.Length ? 4 : lines.Length)..];
        }
        Console.WriteLine(machines.Sum(machine =>
        {
            Console.WriteLine(machine.Prize.x);
            return LetsPlayAGame(machine, (0, 0), 0, (0, 0), null, []) ?? 0;
        }));
    }

    static (int, int) GetFirstTwoInts(string inp)
    {
        var rMatch = Regex.Matches(inp, "\\d+").Select(match => (int.Parse(match.Value))).ToList();
        return (rMatch[0], rMatch[1]);
    }
    static int? LetsPlayAGame(Machine machine, (int x, int y) position, int tokens, (int a, int b) steps, int? cheapestRun, Dictionary<string, int> cheapestPos)
    {
        //win
        if (position.x == machine.Prize.x && position.y == machine.Prize.y)
        {
            cheapestRun = tokens;
            return tokens;
        }

        if (position.x > machine.Prize.x || position.y > machine.Prize.y || (cheapestRun.HasValue && tokens > cheapestRun)) return null;

        var dictKey = $"{position.x}-{position.y}-{steps}";
        if (cheapestPos.TryGetValue(dictKey, out var result))
        {
            if (result < tokens) return null;
        }
        else
        {
            cheapestPos.Add(dictKey, tokens);
        }

        var paths = new List<int?>();

        var aPos = (position.x + machine.ButtonA.x, position.y + machine.ButtonA.y);
        void tryAddA()
        {
            if (steps.a < 100) paths.Add(LetsPlayAGame(machine, aPos, tokens + 3, (steps.a + 1, steps.b), cheapestRun, cheapestPos));
        }
        var bPos = (position.x + machine.ButtonB.x, position.y + machine.ButtonB.y);
        void tryAddB()
        {
            if (steps.b < 100) paths.Add(LetsPlayAGame(machine, bPos, tokens + 1, (steps.a, steps.b + 1), cheapestRun, cheapestPos));
        }
        if (GetManhattanDistance(machine.Prize, aPos) < GetManhattanDistance(machine.Prize, bPos))
        {
            tryAddA();
            tryAddB();
        }
        else
        {
            tryAddB();
            tryAddA();
        }
        return paths.Where(v => v.HasValue).OrderBy(v => v.Value).FirstOrDefault();
    }
    static int GetManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
    {
        return (pos1.x - pos2.x) + (pos1.y - pos2.y);
    }
    class Machine()
    {
        public (int x, int y) ButtonA;
        public (int x, int y) ButtonB;
        public (int x, int y) Prize;
    }
}