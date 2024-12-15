using System.Diagnostics;

namespace CommonLib
{
    public abstract class BaseTask
    {
        public string[] Input { get; }
        public BaseTask(string[] input)
        {
            Input = input;
        }
        public void Execute()
        {
            var stopwatch = Stopwatch.StartNew();
            Solve();
            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }
        protected abstract void Solve();
    }
}
