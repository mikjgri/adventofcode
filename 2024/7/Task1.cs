using System.Data;

public class Task1(string[] input)
{
    public void Solve()
    {
        var equations = input
            .Select(line => line.Split(':'))
            .Select(parts => (
                (
                    long.Parse(parts[0].Trim()),
                    parts[1].Trim().Split(' ').Select(long.Parse).ToList()
                )
            )).ToList();
        Console.WriteLine(equations.Where(eq => IsPossiblyTrue(eq)).Sum(eq => eq.Item1));

        bool IsPossiblyTrue((long result, List< long> numbers) equation, long current = 0, string mathOperator = null)
        {
            if (equation.numbers.Count == 0)
            {
                return equation.result == current;
            }
            if (mathOperator == "*")
            {
                current *= equation.numbers.First();
            }
            else //+ or nothing
            {
                current += equation.numbers.First();
            }
            equation.numbers = equation.numbers[1..];
            return IsPossiblyTrue(equation, current, "+") || IsPossiblyTrue(equation, current, "*");

        }
    }
}