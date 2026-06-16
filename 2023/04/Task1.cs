public class Task1(string[] input)
{
    private List<(List<int> WinningNumbers, List<int> Numbers)> GetCards()
    {
        List<int> getNumbers(string numberSet) => numberSet.Trim().Split(' ').Where(item => !string.IsNullOrEmpty(item)).Select(x => int.Parse(x.Trim())).ToList();
        return input.Select(line =>
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