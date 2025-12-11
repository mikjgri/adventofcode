using System.Diagnostics;

namespace CommonLib;

public abstract class BaseTask
{
    public string DerivedFullName() => GetType().FullName;
    public void Execute()
    {
        Console.WriteLine(DerivedFullName());
        var stopwatch = Stopwatch.StartNew();
        var result = Solve();
        stopwatch.Stop();
        Console.WriteLine($"Answer: {result}, Execution Time: {stopwatch.ElapsedMilliseconds} ms\n");
    }
    protected abstract object Solve();
}
