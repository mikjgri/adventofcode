using CommonLib;

public class Task1(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var numbers = input.Select(line => long.Parse(line)).ToList();
        Console.WriteLine(numbers.Sum(number =>
        {
            var n = number;
            for (var i = 0; i < 2000; i++)
            {
                n = getEvolvedNumber(n);
            }
            return n;
        }));
        long getEvolvedNumber(long secretNumber)
        {
            secretNumber = prune(mix(secretNumber * 64, secretNumber));
            secretNumber = mix(secretNumber / 32, secretNumber);
            secretNumber = prune(mix(secretNumber * 2048, secretNumber));
            return secretNumber;

            long mix(long givenValue, long secretNumber)
            {
                return givenValue ^ secretNumber;
            }
            long prune(long secretNumber)
            {
                return secretNumber % 16777216;
            }
        }
    }
}
