using LostMonkey.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostMonkey.UI
{
    /// <summary>
    /// Toggles a pause overlay with Escape and freezes the game via Time.timeScale.
    /// Wire the Resume/Restart/Menu buttons to the public methods.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

        private bool _isPaused;

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                if (_isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Pause()
        {
            _isPaused = true;
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            _isPaused = false;
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            Time.timeScale = 1f;
        }

        public void OnRestart()
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

        private void OnDestroy()
        {
            // Ensure we never leave the game frozen if the pause UI is torn down.
            Time.timeScale = 1f;
        }
    }
}
