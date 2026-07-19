using LostMonkey.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LostMonkey.UI
{
    /// <summary>
    /// Builds a grid of level buttons, locking those the player hasn't unlocked
    /// yet (per <see cref="GameManager.Progress"/>). Instantiates one button per
    /// level from a prefab into a container.
    /// </summary>
    public class LevelSelectController : MonoBehaviour
    {
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private Button buttonPrefab;

        private void Start()
        {
            if (buttonContainer == null || buttonPrefab == null)
            {
                Debug.LogWarning("[LostMonkey] LevelSelect not wired: assign container and button prefab.");
                return;
            }

            for (int level = 1; level <= GameConstants.TotalLevels; level++)
            {
                CreateLevelButton(level);
            }
        }

        private void CreateLevelButton(int level)
        {
            Button button = Instantiate(buttonPrefab, buttonContainer);
            button.name = $"LevelButton_{level:00}";

            var label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                label.text = level.ToString();
            }

            bool unlocked = IsUnlocked(level);
            button.interactable = unlocked;

            int captured = level;
            button.onClick.AddListener(() => LoadLevel(captured));
        }

        private bool IsUnlocked(int level)
        {
            if (GameManager.Instance != null)
            {
                return GameManager.Instance.Progress.IsLevelUnlocked(level);
            }
            return level == 1; // no save system yet -> only first level
        }

        private void LoadLevel(int level)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadLevel(level);
            }
            else
            {
                SceneManager.LoadScene(GameConstants.LevelSceneName(level));
            }
        }
    }
}
