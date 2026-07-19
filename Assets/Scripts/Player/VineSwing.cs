using UnityEngine;

namespace LostMonkey.Player
{
    /// <summary>
    /// The signature "monkey" mechanic: grab a nearby vine anchor and swing on it
    /// like a pendulum, then release to launch with the momentum you built up.
    ///
    /// Implemented with a runtime <see cref="DistanceJoint2D"/> anchored to a fixed
    /// vine point. While swinging, <see cref="PlayerController2D"/> control is
    /// suspended so the two systems don't fight over the Rigidbody2D.
    ///
    /// Vine anchors are any collider on <see cref="vineLayer"/> (e.g. an empty
    /// GameObject at the top of a vine with a small trigger collider).
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class VineSwing : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private LayerMask vineLayer;
        [Tooltip("How close the monkey must be to a vine anchor to grab it.")]
        [SerializeField] private float grabRadius = 2.5f;
        [SerializeField] private KeyCode grabKey = KeyCode.LeftShift;

        [Header("Swing feel")]
        [Tooltip("Sideways force applied from horizontal input while swinging.")]
        [SerializeField] private float swingForce = 25f;
        [Tooltip("Extra upward impulse added when releasing the vine.")]
        [SerializeField] private float releaseBoost = 4f;

        private Rigidbody2D _body;
        private PlayerController2D _controller;
        private DistanceJoint2D _joint;
        private bool _isSwinging;

        public bool IsSwinging => _isSwinging;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<PlayerController2D>();
        }

        private void Update()
        {
            if (!_isSwinging && Input.GetKeyDown(grabKey))
            {
                TryGrab();
            }
            else if (_isSwinging && (Input.GetKeyUp(grabKey) || Input.GetButtonDown("Jump")))
            {
                Release();
            }
        }

        private void FixedUpdate()
        {
            if (!_isSwinging)
            {
                return;
            }
            float input = Input.GetAxisRaw("Horizontal");
            _body.AddForce(new Vector2(input * swingForce, 0f));
        }

        private void TryGrab()
        {
            Collider2D anchor = Physics2D.OverlapCircle(transform.position, grabRadius, vineLayer);
            if (anchor == null)
            {
                return;
            }

            Vector2 anchorPoint = anchor.transform.position;
            _joint = gameObject.AddComponent<DistanceJoint2D>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.autoConfigureDistance = false;
            _joint.connectedBody = null;
            _joint.connectedAnchor = anchorPoint;
            _joint.distance = Vector2.Distance(transform.position, anchorPoint);
            _joint.maxDistanceOnly = false;
            _joint.enableCollision = false;

            _isSwinging = true;
            if (_controller != null)
            {
                _controller.ControlEnabled = false;
            }
        }

        private void Release()
        {
            if (_joint != null)
            {
                Destroy(_joint);
                _joint = null;
            }
            _isSwinging = false;

            // Preserve swing momentum and add a small upward boost.
            _body.velocity += Vector2.up * releaseBoost;

            if (_controller != null)
            {
                _controller.ControlEnabled = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, grabRadius);
        }
    }
}
