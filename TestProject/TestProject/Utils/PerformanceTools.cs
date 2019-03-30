using System;
using System.Diagnostics;

namespace TestProject.Utils
{
    public static class PerformanceTools
    {
        public static TimeSpan MeasureElapsedTime(Action action)
        {
            var stopWatch = Stopwatch.StartNew();
            action();
            stopWatch.Stop();

            //Console.WriteLine(stopWatch.Elapsed);

            return stopWatch.Elapsed;
        }
    }
}
