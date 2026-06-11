using CommonLib.Solvers;

public class Task2(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var rules = input[2..].Select(line =>
        {
            var s = line.Split("->", StringSplitOptions.TrimEntries);
            return new Rule()
            {
                OriginalPair = s[0],
                ReplacementPairs = [
                    s[0][0] + s[1],
                    s[1] + s[0][1]
                ]
            };
        }).ToList();

        var polymerTemplate = input[0];
        var polymerDict = new Dictionary<string, long>();
        for (var i = 0; i < polymerTemplate.Length-1; i ++) //initially populate dict
        {
            var key = polymerTemplate[i].ToString() + polymerTemplate[i + 1].ToString();
            polymerDict.TryAdd(key, 0);
            polymerDict[key]++;
        }

        for (var i = 0; i < 40; i++)
        {
            var newPolymerDict = new Dictionary<string, long>();
            foreach (var d in polymerDict)
            {
                var matchingRule = rules.FirstOrDefault(r => r.OriginalPair == d.Key);
                if (matchingRule != null)
                {
                    newPolymerDict.TryAdd(matchingRule.ReplacementPairs[0], 0);
                    newPolymerDict.TryAdd(matchingRule.ReplacementPairs[1], 0);
                    newPolymerDict[matchingRule.ReplacementPairs[0]]+=d.Value;
                    newPolymerDict[matchingRule.ReplacementPairs[1]]+=d.Value;
                }
                else
                {
                    throw new Exception("Oh noes");
                }
            }
            polymerDict = newPolymerDict;
        }

        var letterCounts = new Dictionary<char, long>();
        foreach (var pd in polymerDict)
        {
            letterCounts.TryAdd(pd.Key[0], 0);
            letterCounts.TryAdd(pd.Key[1], 0);

            letterCounts[pd.Key[0]] += pd.Value;
            letterCounts[pd.Key[1]] += pd.Value;
        }

        // add 1 to first and last chars
        letterCounts[polymerTemplate[0]]++;
        letterCounts[polymerTemplate[^1]]++;


        return letterCounts.Max(l => l.Value / 2) - letterCounts.Min(l => l.Value /2);
    }
    record Rule
    {
        public required string OriginalPair;
        public required string[] ReplacementPairs;
    }
}