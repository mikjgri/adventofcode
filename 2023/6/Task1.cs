using System.Text.RegularExpressions;

public class Task1
{
    private string[] _input;
    public Task1(string[] input)
    {
        _input = input;
    }
    private List<(int MaxTime, int Distance)> GetRaces()
    {
        string trimAllDoubleSpaces(string input) => Regex.Replace(input, @"\s+", " ");
        List<int> getInts(string input) => input.Split(":")[1].Trim().Split(" ").Select(item => int.Parse(item)).ToList();
        var times = getInts(trimAllDoubleSpaces(_input[0]));
        var distances = getInts(trimAllDoubleSpaces(_input[1]));
        return times.Select(item => (item, distances[times.IndexOf(item)])).ToList();
    }
    private int? Simulate(int maxTime, int speed, int distance)
    {
        var timeRequired = distance / speed;
        return (timeRequired<maxTime ? timeRequired : null);
    }
    public void Solve()
    {
        var races = GetRaces();

        //var result = 1;
        //foreach(var race in races)
        //{
        //    var possibleWins = 0;
        //    for (var i = 1; i < race.MaxTime; i++)
        //    {
        //        var simulatedTime = Simulate(race.MaxTime - i, i, race.Distance);
        //        if (simulatedTime.HasValue && simulatedTime.Value <= race.MaxTime)
        //        {
        //            possibleWins++;
        //        }
        //    }
        //    result *= possibleWins;
        //}

        var result = races.Select(race => Enumerable.Range(1, race.MaxTime).Select(startTime => Simulate(race.MaxTime - startTime, startTime, race.Distance)).Where(raceTime => raceTime.HasValue).Count()).Aggregate((a, b) => a * b);

        Console.WriteLine(result);
    }
}