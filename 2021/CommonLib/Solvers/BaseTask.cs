using System.Diagnostics;

namespace CommonLib.Solvers;

public abstract class BaseTask
{
    public string DerivedFullName() => GetType().FullName;
    public string AssemblyName => GetType().Assembly.GetName().Name!;
    public void Execute()
    {
        Console.WriteLine($"Day {AssemblyName} - {DerivedFullName()}");
        var stopwatch = Stopwatch.StartNew();
        var result = Solve();
        stopwatch.Stop();
        Console.WriteLine($"Answer: {result}, Execution Time: {stopwatch.ElapsedMilliseconds} ms\n");
    }
    protected abstract object Solve();
}
