using System;
using System.Diagnostics;

namespace TestAssembly.Utils
{
    public static class PerformanceTools
    {
        public static TimeSpan MeasureElapsedTime(Action action)
        {
            var stopWatch = Stopwatch.StartNew();
            action();
            stopWatch.Stop();

            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed}");

            return stopWatch.Elapsed;
        }
    }
}
