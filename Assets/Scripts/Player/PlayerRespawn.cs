using System.Collections;
using LostMonkey.Audio;
using UnityEngine;

namespace LostMonkey.Player
{
    /// <summary>
    /// Handles death and respawn to the last activated checkpoint. Hazards and
    /// kill zones call <see cref="Die"/>; checkpoints call <see cref="SetCheckpoint"/>.
    /// Non-punishing by design (no lives system) — respawn is instant.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerRespawn : MonoBehaviour
    {
        [Tooltip("Brief invulnerability window after respawning, in seconds.")]
        [SerializeField] private float respawnInvulnerability = 0.5f;

        private Rigidbody2D _body;
        private Vector3 _checkpoint;
        private bool _isDead;
        private bool _isInvulnerable;

        public bool IsInvulnerable => _isInvulnerable;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _checkpoint = transform.position; // level start is the first checkpoint
        }

        public void SetCheckpoint(Vector3 position)
        {
            _checkpoint = position;
        }

        public void Die()
        {
            if (_isDead || _isInvulnerable)
            {
                return;
            }
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            _isDead = true;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDeath();
            }

            // Stop motion and teleport to the checkpoint.
            _body.velocity = Vector2.zero;
            transform.position = _checkpoint;

            _isDead = false;
            _isInvulnerable = true;
            yield return new WaitForSeconds(respawnInvulnerability);
            _isInvulnerable = false;
        }
    }
}
