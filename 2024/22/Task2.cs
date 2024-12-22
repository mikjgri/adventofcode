using CommonLib;
using System.Collections.Concurrent;
public class Task2(string[] input) : BaseTask()
{
    protected override void Solve()
    {
        var numbers = input.Select(line => long.Parse(line)).ToList();

        var changeResLis = numbers.Select(number =>
        {
            var numberChanges = new List<(int result, int? change)>
            {
                (getLastDigit(number), null)
            };
            var n = number;
            for (var i = 0; i < 2000; i++)
            {
                n = getEvolvedNumber(n);
                var lastDigit = getLastDigit(n);
                numberChanges.Add((lastDigit, lastDigit - numberChanges.Last().result));
            }
            return numberChanges.SelectWithIndex().ToList();
        }).ToList();


        var finalLists = changeResLis.Select(crl =>
        {
            //var l1 = new List<(int result, string changes)>();
            var l1 = new Dictionary<string, int>();
            foreach (var (item, index) in crl[4..])
            {
                var prev4 = crl[(index - 3)..(index + 1)].Select(s => s.item.change).ToList();
                var changes = string.Join(',', prev4);
                if (!l1.ContainsKey(changes))
                {
                    l1.Add(changes, item.result);
                }
            }
            return l1;
        }).SelectWithIndex().ToList();

        var scores = new ConcurrentBag<int>();
        Parallel.ForEach(finalLists, finalList =>
        {
            var otherLists = finalLists.Where(fn => fn.index != finalList.index);
            var highestScore = 0;
            foreach (var chain in finalList.item)
            {
                var result = chain.Value;
                result += otherLists.Sum(ol =>
                {
                    return (ol.item.TryGetValue(chain.Key, out var olValue) ? olValue : 0);
                });
                if (result > highestScore)
                {
                    highestScore = result;
                }
            }
            scores.Add(highestScore);
            Console.WriteLine($"{scores.Count}/{finalLists.Count}");
        });
        Console.WriteLine(scores.Max());


        int getLastDigit(long number)
        {
            return int.Parse(number.ToString()[^1].ToString());
        }
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
