using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        List<(char opening, char closing, int score)> pairs =
        [
            ('(',')', 1),
            ('[',']', 2),
            ('{','}', 3),
            ('<','>', 4)
        ];
        var incompleteLines = new List<string>();
        foreach (var item in input)
        {
            var wItem = item;
            bool hasCorruptChar = false;
            while (!hasCorruptChar && !string.IsNullOrEmpty(wItem))
            {
                var foundClosing = false;
                for (var i = 0; i < wItem.Length - 1; i++)
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
                            hasCorruptChar = true;
                            break;
                        }
                    }
                }
                if (!foundClosing && !hasCorruptChar)
                {
                    incompleteLines.Add(wItem);
                    break;
                }
            }
        }

        var scores = incompleteLines.Select(line =>
        {
            var chars = line.Reverse().ToArray();
            long sum = 0;
            foreach (var c in chars)
            {
                sum *= 5;
                sum += pairs.First(p => p.opening == c).score;
            }
            return sum;
        }).Order().ToArray();

        return scores[scores.Length/2];
    }
}