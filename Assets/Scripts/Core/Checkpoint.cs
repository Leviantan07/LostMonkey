using LostMonkey.Audio;
using LostMonkey.Player;
using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// Activates when the player passes through, recording the respawn point.
    /// Fires once. Requires a trigger Collider2D.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        [Tooltip("Optional visual toggled on when this checkpoint activates.")]
        [SerializeField] private GameObject activatedVisual;

        private bool _activated;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_activated || !other.CompareTag(GameConstants.PlayerTag))
            {
                return;
            }
            if (!other.TryGetComponent(out PlayerRespawn respawn))
            {
                return;
            }

            _activated = true;
            respawn.SetCheckpoint(transform.position);

            if (activatedVisual != null)
            {
                activatedVisual.SetActive(true);
            }
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCheckpoint();
            }
        }
    }
}
