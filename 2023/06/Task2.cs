public class Task2(string[] input)
{
    private (double MaxTime, double Distance) GetRace()
    {
        double getNumber(string input) => double.Parse(input.Split(":")[1].Trim().Replace(" ",""));
        var time = getNumber(input[0]);
        var distance = getNumber(input[1]);
        return (time, distance);
    }
    private double? Simulate(double maxTime, double speed, double distance)
    {
        var timeRequired = distance / speed;
        return (timeRequired<maxTime ? timeRequired : null);
    }
    public void Solve()
    {
        var race = GetRace();

        var result = Enumerable.Range(1, (int)race.MaxTime).Select(startTime => Simulate(race.MaxTime - startTime, startTime, race.Distance)).Where(raceTime => raceTime.HasValue).Count();

        Console.WriteLine(result);
    }
}