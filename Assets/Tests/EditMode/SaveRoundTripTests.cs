using LostMonkey.Core.Progression;
using NUnit.Framework;

namespace LostMonkey.Tests
{
    public class SaveRoundTripTests
    {
        private const int TotalLevels = 8;

        [Test]
        public void Save_ThenLoad_RestoresProgress()
        {
            var store = new InMemorySaveStore();

            var progress = store.Load(TotalLevels);
            progress.RecordLevelCompleted(1, 2, TotalLevels);
            progress.RecordLevelCompleted(2, 3, TotalLevels);
            store.Save(progress);

            ProgressData reloaded = store.Load(TotalLevels);
            Assert.AreEqual(3, reloaded.HighestUnlockedLevel);
            Assert.AreEqual(2, reloaded.GetBestBananas(1));
            Assert.AreEqual(3, reloaded.GetBestBananas(2));
        }

        [Test]
        public void Clear_ResetsToDefault()
        {
            var store = new InMemorySaveStore();
            var progress = store.Load(TotalLevels);
            progress.RecordLevelCompleted(1, 3, TotalLevels);
            store.Save(progress);

            store.Clear();

            ProgressData reloaded = store.Load(TotalLevels);
            Assert.AreEqual(1, reloaded.HighestUnlockedLevel);
            Assert.AreEqual(0, reloaded.GetBestBananas(1));
        }
    }
}
