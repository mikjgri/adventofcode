using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var polymerTemplate = input[0].Select(c => c).ToList();

        var rules = input[2..].Select(line =>
        {
            var s = line.Split("->", StringSplitOptions.TrimEntries);
            return new Rule()
            {
                Pair = (s[0][0], s[0][1]),
                Insert = s[1][0]
            };
        });

        for(var i = 0; i<10; i++)
        {
            var index = 0;
            while (index < polymerTemplate.Count-1)
            {
                index++;
                var pair = (polymerTemplate[index-1], polymerTemplate[index]);
                var machingRule = rules.FirstOrDefault(r => r.Pair == pair);
                if (machingRule != null)
                {
                    polymerTemplate.Insert(index, machingRule.Insert);
                    index++;
                }
            }
        }
        var sums = polymerTemplate.Distinct().Select(pt => polymerTemplate.Count(ptS => ptS == pt)).ToList();

        return sums.Max()-sums.Min();
    }
    record Rule
    {
        public required (char first, char second) Pair;
        public required char Insert;
    }
}