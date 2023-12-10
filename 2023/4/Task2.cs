public class Task2(string[] input)
{
    private List<(int Id, List<int> WinningNumbers, List<int> Numbers)> GetCards()
    {
        List<int> getNumbers(string numberSet) => numberSet.Trim().Split(' ').Where(item => !string.IsNullOrEmpty(item)).Select(x => int.Parse(x.Trim())).ToList();
        return input.Select(line =>
        {
            var numberSets = line.Split(":")[1].Split("|");
            return (input.ToList().IndexOf(line)+1, getNumbers(numberSets[0]), getNumbers(numberSets[1]));
        }).ToList();
    }
    public void Solve()
    {
        var originalCards = GetCards();

        var cardStack = new Stack<(int Id, List<int> WinningNumbers, List<int> Numbers)>(originalCards);
        var result = 0;
        while (cardStack.Any())
        {
            result++;
            var (Id, WinningNumbers, Numbers) = cardStack.Pop();
            var wins = Numbers.Where(num => WinningNumbers.Contains(num)).Count();
            for ( var i = 1; i <= wins; i++)
            {
                cardStack.Push(originalCards[Id+i-1]);
            }
        }
        Console.WriteLine(result);
    }
}