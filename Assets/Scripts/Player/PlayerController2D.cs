using UnityEngine;

namespace LostMonkey.Player
{
    /// <summary>
    /// Starter 2D platformer controller for Lost Monkey.
    ///
    /// Implements run + jump with two "game feel" staples:
    ///   * Coyote time      — you can still jump for a few frames after leaving a ledge.
    ///   * Jump buffering    — a jump pressed slightly before landing still fires.
    ///
    /// This is a starting template (Design Pillar #1: movement feels great first).
    /// It uses Unity's legacy Input API so it works in a fresh project with no
    /// extra package setup. Attach to the player GameObject, which needs a
    /// Rigidbody2D and a Collider2D, and assign a ground-check transform + layer.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float jumpForce = 15f;

        [Header("Ground check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Game feel")]
        [Tooltip("Seconds you can still jump after leaving the ground.")]
        [SerializeField] private float coyoteTime = 0.1f;
        [Tooltip("Seconds a jump press is remembered before landing.")]
        [SerializeField] private float jumpBufferTime = 0.1f;

        private Rigidbody2D _body;
        private float _horizontalInput;
        private float _coyoteCounter;
        private float _jumpBufferCounter;
        private bool _isFacingRight = true;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");

            // Buffer the jump press so it survives a few frames until grounded.
            if (Input.GetButtonDown("Jump"))
            {
                _jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            UpdateCoyoteTimer();
            TryJump();
            FlipToFaceMovement();
        }

        private void FixedUpdate()
        {
            _body.velocity = new Vector2(_horizontalInput * moveSpeed, _body.velocity.y);
        }

        private void UpdateCoyoteTimer()
        {
            _coyoteCounter = IsGrounded() ? coyoteTime : _coyoteCounter - Time.deltaTime;
        }

        private void TryJump()
        {
            if (_jumpBufferCounter > 0f && _coyoteCounter > 0f)
            {
                _body.velocity = new Vector2(_body.velocity.x, jumpForce);
                _jumpBufferCounter = 0f;
                _coyoteCounter = 0f;
            }
        }

        private bool IsGrounded()
        {
            if (groundCheck == null)
            {
                return false;
            }
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        private void FlipToFaceMovement()
        {
            if ((_horizontalInput > 0f && !_isFacingRight) || (_horizontalInput < 0f && _isFacingRight))
            {
                _isFacingRight = !_isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }

        // Visualise the ground-check probe in the editor.
        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null)
            {
                return;
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
