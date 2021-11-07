using System;

namespace RhymeDictionary.Migration
{
    internal class ConsoleWritingProgress : IProgress<(int, int)>
    {
        private readonly TimeSpan period;
        private DateTime lastPrint = DateTime.MinValue;

        public ConsoleWritingProgress(TimeSpan period)
        {
            this.period = period;
        }

        public void Report((int, int) value)
        {
            var (position, total) = value;
            var percentage = position * 100m / total;

            var now = DateTime.Now;
            if (now - lastPrint < period)
            {
                return;
            }

            lastPrint = now;
            Console.WriteLine($"Migrated {percentage:N2}% ({position}/{total})");
        }
    }
}