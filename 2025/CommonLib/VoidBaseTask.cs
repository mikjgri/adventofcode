using System.Diagnostics;

namespace CommonLib;

public abstract class VoidBaseTask
{
    public string DerivedFullName() => GetType().FullName;
    public void Execute()
    {
        Console.WriteLine(DerivedFullName());
        var stopwatch = Stopwatch.StartNew();
        Solve();
        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms\n");
    }
    protected abstract void Solve();
}
