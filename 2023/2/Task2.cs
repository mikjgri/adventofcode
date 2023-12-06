public class Task2
{
    private string[] _input;
    public Task2(string[] input)
    {
        _input = input;
    }
    private List<List<List<(int count, string color)>>> GetGames()
    {
        var games = new List<List<List<(int count, string color)>>>();

        foreach (var line in _input)
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
    public void Solve()
    {
        var games = GetGames();

        var result = games.Sum(game =>
        {
            var gameRes = 1;
            foreach (var color in new string[] { "red", "blue", "green" })
            {
                gameRes *= game.Max(set => set.Where(item => item.color == color).Sum(item => item.count));
            }
            return gameRes;
        });
        Console.WriteLine(result);
    }
}