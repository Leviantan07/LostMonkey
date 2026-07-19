using System;
using System.Collections.Generic;

namespace LostMonkey.Core.Progression
{
    /// <summary>
    /// Pure C# model of the player's persistent progress: which levels are
    /// unlocked and the best banana count per level. No Unity dependency, so it
    /// is fully unit-testable. Persisted through an <see cref="ISaveStore"/>.
    /// </summary>
    public class ProgressData
    {
        /// <summary>Highest unlocked level number (1-based). Level 1 is always unlocked.</summary>
        public int HighestUnlockedLevel { get; private set; } = 1;

        private readonly Dictionary<int, int> _bestBananas = new Dictionary<int, int>();

        public bool IsLevelUnlocked(int levelNumber)
        {
            return levelNumber >= 1 && levelNumber <= HighestUnlockedLevel;
        }

        public int GetBestBananas(int levelNumber)
        {
            return _bestBananas.TryGetValue(levelNumber, out int best) ? best : 0;
        }

        /// <summary>
        /// Records a completed level: unlocks the next one and keeps the best
        /// banana score seen so far for that level.
        /// </summary>
        public void RecordLevelCompleted(int levelNumber, int bananasCollected, int totalLevels)
        {
            if (levelNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(levelNumber));
            }

            int nextLevel = Math.Min(levelNumber + 1, totalLevels);
            if (nextLevel > HighestUnlockedLevel)
            {
                HighestUnlockedLevel = nextLevel;
            }

            if (bananasCollected > GetBestBananas(levelNumber))
            {
                _bestBananas[levelNumber] = bananasCollected;
            }
        }

        // ---- Serialization helpers (used by ISaveStore implementations) ------

        public IReadOnlyDictionary<int, int> BestBananasSnapshot => _bestBananas;

        public void LoadFrom(int highestUnlockedLevel, IReadOnlyDictionary<int, int> bestBananas)
        {
            HighestUnlockedLevel = Math.Max(1, highestUnlockedLevel);
            _bestBananas.Clear();
            if (bestBananas != null)
            {
                foreach (KeyValuePair<int, int> entry in bestBananas)
                {
                    _bestBananas[entry.Key] = entry.Value;
                }
            }
        }
    }
}
