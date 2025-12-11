using CommonLib.Solvers;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var nodes = input.Select(line =>
        {
            var split = line.Split(": ");
            return (split[0], split[1].Split(" ").Select(str => str).ToArray());
        }).ToDictionary(a => a.Item1, a => a.Item2);
        nodes.Add("out", []);

        var result = 0;

        RunToOut("you");
        return result;

        void RunToOut(string nodeId)
        {
            if (nodeId == "out")
            {
                result++;
                return;
            }
            foreach (var child in nodes[nodeId])
            {
                RunToOut(child);
            }
        }
    }
}