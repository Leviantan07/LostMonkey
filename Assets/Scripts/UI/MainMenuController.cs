using LostMonkey.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostMonkey.UI
{
    /// <summary>
    /// Main menu buttons. Wire Play/Quit to these methods in the Inspector.
    /// Falls back to direct scene loads if no persistent GameManager exists yet.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        public void OnPlay()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadLevelSelect();
            }
            else
            {
                SceneManager.LoadScene(GameConstants.LevelSelectScene);
            }
        }

        public void OnQuit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
