public class Task1(string[] input)
{
    private List<List<List<(int count, string color)>>> GetGames()
    {
        var games = new List<List<List<(int count, string color)>>>();

        foreach (var line in input)
        {
            var retGame = new List<List<(int count, string color)>>();
            var game = line.Split(':')[1];
            var sets = game.Split(";");
            foreach (var set in sets)
            {
                var retSet = new List<(int count, string color)>();
                var cubes = set.Split(",");
                foreach (var cube in cubes)
                {
                    var split = cube.Trim().Split(" ");
                    retSet.Add((int.Parse(split[0]), split[1]));
                }
                retGame.Add(retSet);
            }
            games.Add(retGame);
        }
        return games;
    }
    private bool IsValidSet(List<(int count, string color)> set)
    {
        var maxC = new Dictionary<string, int>()
            {
                { "red" , 12},
                { "green" , 13},
                { "blue" , 14}
            };
        foreach (var max in maxC)
        {
            if (set.Where(item => item.color == max.Key).Sum(item => item.count) > max.Value)
            {
                return false;
            }
        }
        return true;
    }
    public void Solve()
    {
        var games = GetGames();

        int result = 0;
        foreach (var game in games)
        {
            if (game.All(IsValidSet))
            {
                result += games.IndexOf(game) + 1;
            }
        }
        Console.WriteLine(result);
    }
}