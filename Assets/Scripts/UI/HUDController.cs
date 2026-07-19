using LostMonkey.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LostMonkey.UI
{
    /// <summary>
    /// In-level heads-up display. Shows the banana count and keeps it in sync with
    /// the <see cref="LevelManager"/>.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private Text bananaText;

        private void Start()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.BananasChanged += OnBananasChanged;
                OnBananasChanged(LevelManager.Instance.BananasCollected, LevelManager.Instance.BananasTotal);
            }
        }

        private void OnDestroy()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.BananasChanged -= OnBananasChanged;
            }
        }

        private void OnBananasChanged(int collected, int total)
        {
            if (bananaText != null)
            {
                bananaText.text = $"Bananas: {collected}/{total}";
            }
        }
    }
}
