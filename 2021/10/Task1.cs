using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<(char opening, char closing, int score)> pairs =
        [
            ('(',')', 3),
            ('[',']', 57),
            ('{','}', 1197),
            ('<','>', 25137)
        ];
        long sum = 0;
        foreach (var item in input)
        {
            var wItem = item;
            char? corruptChar = null;
            while (corruptChar == null && !string.IsNullOrEmpty(wItem))
            {
                var foundClosing = false;
                for (var i = 0; i < wItem.Length-1; i++)
                {
                    var cCurrent = wItem[i];
                    var cNext = wItem[i + 1];

                    if (pairs.Any(p => p.closing == cNext)) //next is closing
                    {
                        foundClosing = true;
                        var currentPair = pairs.FirstOrDefault(p => p.opening == cCurrent);

                        if (cNext == currentPair.closing) //expected closing value. happy, remove pair
                        {
                            var subStr = wItem.Remove(i, 2);
                            wItem = subStr;
                            break;
                        }
                        else
                        {
                            //Console.WriteLine($"Expected {currentPair.closing}, but found {cNext} instead.");
                            corruptChar = cNext;
                            break;
                        }
                    }
                }
                if (!foundClosing && corruptChar == null)
                {
                    break;
                }
            }

            if (corruptChar.HasValue)
            {
                sum += pairs.First(p => corruptChar.Value == p.opening || corruptChar.Value == p.closing).score;
            }
        }
        return sum;
    }
}