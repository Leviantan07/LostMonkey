using LostMonkey.Core.Progression;
using NUnit.Framework;

namespace LostMonkey.Tests
{
    public class ProgressDataTests
    {
        private const int TotalLevels = 8;

        [Test]
        public void Level1_UnlockedByDefault()
        {
            var progress = new ProgressData();
            Assert.IsTrue(progress.IsLevelUnlocked(1));
            Assert.IsFalse(progress.IsLevelUnlocked(2));
        }

        [Test]
        public void CompletingLevel_UnlocksNext()
        {
            var progress = new ProgressData();
            progress.RecordLevelCompleted(1, 2, TotalLevels);
            Assert.IsTrue(progress.IsLevelUnlocked(2));
            Assert.IsFalse(progress.IsLevelUnlocked(3));
        }

        [Test]
        public void CompletingLastLevel_DoesNotExceedTotal()
        {
            var progress = new ProgressData();
            progress.RecordLevelCompleted(TotalLevels, 3, TotalLevels);
            Assert.AreEqual(TotalLevels, progress.HighestUnlockedLevel);
            Assert.IsFalse(progress.IsLevelUnlocked(TotalLevels + 1));
        }

        [Test]
        public void BestBananas_KeepsMaximum()
        {
            var progress = new ProgressData();
            progress.RecordLevelCompleted(1, 1, TotalLevels);
            progress.RecordLevelCompleted(1, 3, TotalLevels);
            progress.RecordLevelCompleted(1, 2, TotalLevels);
            Assert.AreEqual(3, progress.GetBestBananas(1));
        }

        [Test]
        public void ReplayingEarlierLevel_DoesNotLockLaterLevels()
        {
            var progress = new ProgressData();
            progress.RecordLevelCompleted(1, 3, TotalLevels);
            progress.RecordLevelCompleted(2, 3, TotalLevels); // highest = 3
            progress.RecordLevelCompleted(1, 3, TotalLevels); // replay level 1
            Assert.AreEqual(3, progress.HighestUnlockedLevel);
        }
    }
}
