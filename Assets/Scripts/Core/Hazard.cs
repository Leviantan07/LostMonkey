using LostMonkey.Player;
using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// Anything that kills the player on contact (spikes, saws, ...). Works with
    /// either a trigger or a solid collider. Respawns the player at the last
    /// checkpoint via <see cref="PlayerRespawn"/>.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) => TryKill(other);

        private void OnCollisionEnter2D(Collision2D collision) => TryKill(collision.collider);

        private void TryKill(Collider2D other)
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
