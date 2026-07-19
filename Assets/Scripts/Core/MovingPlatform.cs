using System.Collections.Generic;
using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// Moves back and forth between waypoints and carries anything standing on it.
    /// Riders are moved by the platform's per-frame delta (rather than parented),
    /// which avoids scale/parenting issues with the player's facing flip.
    ///
    /// Use a kinematic Rigidbody2D on the platform for clean 2D physics contacts.
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        [Tooltip("Points the platform travels between, in order (ping-pong).")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float speed = 2f;
        [Tooltip("Seconds to pause at each waypoint.")]
        [SerializeField] private float waitTime = 0.5f;

        private readonly HashSet<Transform> _riders = new HashSet<Transform>();
        private int _targetIndex;
        private int _direction = 1;
        private float _waitTimer;

        private void FixedUpdate()
        {
            if (waypoints == null || waypoints.Length < 2)
            {
                return;
            }

            if (_waitTimer > 0f)
            {
                _waitTimer -= Time.fixedDeltaTime;
                return;
            }

            Vector3 current = transform.position;
            Vector3 target = waypoints[_targetIndex].position;
            Vector3 newPos = Vector3.MoveTowards(current, target, speed * Time.fixedDeltaTime);
            Vector3 delta = newPos - current;

            transform.position = newPos;
            foreach (Transform rider in _riders)
            {
                if (rider != null)
                {
                    rider.position += delta;
                }
            }

            if (Vector3.Distance(newPos, target) < 0.01f)
            {
                AdvanceTarget();
                _waitTimer = waitTime;
            }
        }

        private void AdvanceTarget()
        {
            _targetIndex += _direction;
            if (_targetIndex >= waypoints.Length || _targetIndex < 0)
            {
                _direction *= -1;
                _targetIndex += 2 * _direction;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(GameConstants.PlayerTag))
            {
                _riders.Add(collision.collider.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(GameConstants.PlayerTag))
            {
                _riders.Remove(collision.collider.transform);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (waypoints == null)
            {
                return;
            }
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null)
                {
                    continue;
                }
                Gizmos.DrawWireCube(waypoints[i].position, Vector3.one * 0.3f);
                if (i + 1 < waypoints.Length && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
        }
    }
}
