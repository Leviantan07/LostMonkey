using LostMonkey.Audio;
using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// A banana. When the player overlaps it, it registers with the
    /// <see cref="LevelManager"/>, plays a sound, and disappears.
    /// Requires a trigger Collider2D.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        private bool _collected;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collected || !other.CompareTag(GameConstants.PlayerTag))
            {
                return;
            }
            _collected = true;

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CollectBanana();
            }
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCollect();
            }
            Destroy(gameObject);
        }
    }
}
