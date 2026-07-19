using System;

namespace LostMonkey.Core
{
    /// <summary>
    /// Pure C# banana bookkeeping for a single level. No Unity dependency, so it
    /// is fully unit-testable in EditMode. <see cref="LevelManager"/> owns one.
    /// </summary>
    public class BananaCounter
    {
        public int Total { get; }
        public int Collected { get; private set; }

        public bool IsComplete => Collected >= Total;

        public BananaCounter(int total)
        {
            if (total < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total), "Total cannot be negative.");
            }
            Total = total;
            Collected = 0;
        }

        /// <summary>
        /// Records one banana. Returns false if everything was already collected
        /// (guards against double-counting).
        /// </summary>
        public bool Collect()
        {
            if (Collected >= Total)
            {
                return false;
            }
            Collected++;
            return true;
        }
    }
}
