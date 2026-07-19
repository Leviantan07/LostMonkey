using LostMonkey.Core.Progression;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostMonkey.Core
{
    /// <summary>
    /// Persistent, cross-scene game controller: owns player progress and handles
    /// all scene routing (menu, level select, loading levels). Survives scene
    /// loads via DontDestroyOnLoad and is reachable through <see cref="Instance"/>.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ProgressData Progress { get; private set; }

        private ISaveStore _saveStore;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _saveStore = new PlayerPrefsSaveStore();
            Progress = _saveStore.Load(GameConstants.TotalLevels);
        }

        /// <summary>Called by <see cref="LevelManager"/> when a level's exit is reached.</summary>
        public void NotifyLevelCompleted(int levelNumber, int bananasCollected)
        {
            Progress.RecordLevelCompleted(levelNumber, bananasCollected, GameConstants.TotalLevels);
            _saveStore.Save(Progress);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(GameConstants.MainMenuScene);
        }

        public void LoadLevelSelect()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(GameConstants.LevelSelectScene);
        }

        public void LoadLevel(int levelNumber)
        {
            Time.timeScale = 1f;
            int clamped = Mathf.Clamp(levelNumber, 1, GameConstants.TotalLevels);
            SceneManager.LoadScene(GameConstants.LevelSceneName(clamped));
        }

        public void ReloadCurrentLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel(int currentLevelNumber)
        {
            if (currentLevelNumber >= GameConstants.TotalLevels)
            {
                LoadLevelSelect();
                return;
            }
            LoadLevel(currentLevelNumber + 1);
        }

        /// <summary>Debug/testing helper to wipe saved progress.</summary>
        public void ResetProgress()
        {
            _saveStore.Clear();
            Progress = _saveStore.Load(GameConstants.TotalLevels);
        }
    }
}
