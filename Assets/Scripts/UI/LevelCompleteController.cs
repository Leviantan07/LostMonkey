using LostMonkey.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LostMonkey.UI
{
    /// <summary>
    /// End-of-level panel. Hidden until the <see cref="LevelManager"/> reports the
    /// level complete, then shows the banana result and Next/Replay/Menu buttons.
    /// </summary>
    public class LevelCompleteController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text resultText;

        private void Start()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LevelCompleted += OnLevelCompleted;
            }
        }

        private void OnDestroy()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LevelCompleted -= OnLevelCompleted;
            }
        }

        private void OnLevelCompleted(int collected, int total)
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }
            if (resultText != null)
            {
                resultText.text = $"Level complete!\nBananas: {collected}/{total}";
            }
            Time.timeScale = 0f;
        }

        public void OnNext()
        {
            Time.timeScale = 1f;
            int current = LevelManager.Instance != null ? LevelManager.Instance.LevelNumber : 1;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadNextLevel(current);
            }
            else
            {
                SceneManager.LoadScene(GameConstants.LevelSceneName(current + 1));
            }
        }

        public void OnReplay()
        {
            Time.timeScale = 1f;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReloadCurrentLevel();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void OnMenu()
        {
            Time.timeScale = 1f;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadMainMenu();
            }
            else
            {
                SceneManager.LoadScene(GameConstants.MainMenuScene);
            }
        }
    }
}
