namespace LostMonkey.Core
{
    /// <summary>
    /// Central place for scene names and shared tuning constants so magic
    /// strings don't get scattered across the codebase.
    /// </summary>
    public static class GameConstants
    {
        // Scene names — must match the scene file names added to Build Settings.
        public const string MainMenuScene = "MainMenu";
        public const string LevelSelectScene = "LevelSelect";

        // Level scenes are named "Level_01" ... "Level_08".
        public const string LevelScenePrefix = "Level_";
        public const int TotalLevels = 8;

        // Tags / layers expected in the Unity project.
        public const string PlayerTag = "Player";

        /// <summary>Scene name for a 1-based level number, e.g. 3 -> "Level_03".</summary>
        public static string LevelSceneName(int levelNumber)
        {
            return LevelScenePrefix + levelNumber.ToString("00");
        }
    }
}
