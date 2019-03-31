using System;

namespace TestAssembly.Utils
{
    public static class ThreadSafeRandomGenerator
    {
        private static Random _global = new Random();
        [ThreadStatic]
        private static Random _local;

        public static int Next(int maxValue)
        {
            Random inst = _local;
            if (inst == null)
            {
                int seed;
                lock (_global) seed = _global.Next();
                _local = inst = new Random(seed);
            }
            return inst.Next(maxValue);
        }
    }
}
