namespace CommonLib;

public static class TaskRunner
{
    public static void RunWithStackSize(Action run, int stackMb)
    {
        var thread = new Thread(() =>
        {
            run();
        }, stackMb * 1024 * 1024);
        thread.Start();
        thread.Join();
    }
}