using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// The level goal (the tree / portal home). When the player reaches it, the
    /// level is marked complete via the <see cref="LevelManager"/>.
    /// Requires a trigger Collider2D.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class LevelExit : MonoBehaviour
    {
        private bool _reached;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_reached || !other.CompareTag(GameConstants.PlayerTag))
            {
                return;
            }
            _reached = true;

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CompleteLevel();
            }
        }
    }
}
