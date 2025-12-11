using System.Diagnostics;

namespace CommonLib.Solvers;

public abstract class VoidBaseTask
{
    public string DerivedFullName() => GetType().FullName;
    public string AssemblyName => GetType().Assembly.GetName().Name!;
    public void Execute()
    {
        Console.WriteLine($"Day {AssemblyName} - {DerivedFullName()}");
        var stopwatch = Stopwatch.StartNew();
        Solve();
        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms\n");
    }
    protected abstract void Solve();
}
