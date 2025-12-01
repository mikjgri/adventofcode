using System.Diagnostics;

namespace CommonLib;

public abstract class BaseTask
{
    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        Solve();
        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
    }
    protected abstract void Solve();
}
