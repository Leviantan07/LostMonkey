using LostMonkey.Player;
using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// A large trigger placed below the level to catch falls. Respawns the player
    /// at the last checkpoint. (Kept separate from <see cref="Hazard"/> so a
    /// designer can reason about "the floor of the world" independently.)
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(GameConstants.PlayerTag))
            {
                return;
            }
            if (other.TryGetComponent(out PlayerRespawn respawn))
            {
                respawn.Die();
            }
        }
    }
}
