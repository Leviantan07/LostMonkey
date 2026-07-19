namespace LostMonkey.Core.Progression
{
    /// <summary>
    /// Abstraction over persistence so <see cref="ProgressData"/> can be saved to
    /// PlayerPrefs in the game and to an in-memory store in tests.
    /// </summary>
    public interface ISaveStore
    {
        ProgressData Load(int totalLevels);
        void Save(ProgressData data);
        void Clear();
    }
}
