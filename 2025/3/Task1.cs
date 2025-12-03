using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        Console.WriteLine(input
            .Sum(line => Enumerable.Range(0, line.Length - 1)
                .Max(i => Enumerable.Range(i + 1, line.Length - 1 - i)
                    .Max(j => int.Parse(line[i] + line[j].ToString()))
                )
            )
        );
    }
}