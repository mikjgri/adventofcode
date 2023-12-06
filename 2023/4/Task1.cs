public class Task1
{
    private string[] _input;
    public Task1(string[] input)
    {
        _input = input;
    }
    private List<(List<int> WinningNumbers, List<int> Numbers)> GetCards()
    {
        List<int> getNumbers(string numberSet) => numberSet.Trim().Split(' ').Where(item => !string.IsNullOrEmpty(item)).Select(x => int.Parse(x.Trim())).ToList();
        return _input.Select(line =>
        {
            var numberSets = line.Split(":")[1].Split("|");
            return (getNumbers(numberSets[0]), getNumbers(numberSets[1]));
        }).ToList();
    }
    public void Solve()
    {
        var cards = GetCards();
        var result = cards.Sum(card =>
        {
            var wins = card.Numbers.Where(num => card.WinningNumbers.Contains(num)).Count();
            return Math.Floor(Math.Pow(2, wins - 1));
        });
        Console.WriteLine(result);
    }
}