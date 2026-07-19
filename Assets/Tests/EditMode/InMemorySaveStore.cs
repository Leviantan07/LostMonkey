using System.Collections.Generic;
using LostMonkey.Core.Progression;

namespace LostMonkey.Tests
{
    /// <summary>
    /// Test double for <see cref="ISaveStore"/> that keeps everything in memory,
    /// so progression persistence can be tested without touching PlayerPrefs.
    /// </summary>
    public class InMemorySaveStore : ISaveStore
    {
        private int _highest = 1;
        private Dictionary<int, int> _best = new Dictionary<int, int>();
        private bool _hasData;

        public ProgressData Load(int totalLevels)
        {
            var data = new ProgressData();
            if (_hasData)
            {
                data.LoadFrom(_highest, _best);
            }
            return data;
        }

        public void Save(ProgressData data)
        {
            _highest = data.HighestUnlockedLevel;
            _best = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> entry in data.BestBananasSnapshot)
            {
                _best[entry.Key] = entry.Value;
            }
            _hasData = true;
        }

        public void Clear()
        {
            _highest = 1;
            _best = new Dictionary<int, int>();
            _hasData = false;
        }
    }
}
