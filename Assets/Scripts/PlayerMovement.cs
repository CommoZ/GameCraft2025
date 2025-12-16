using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // --- Inspector Variables ---
    [Header("Movement")]
    [SerializeField] private float speed = 8f;

    [Header("Charged Jump")]
    [SerializeField] private float minJumpPower = 8f;
    [SerializeField] private float maxJumpPower = 16f;
    [SerializeField] private float chargeRate = 1.5f; // units per second

    [Header("Physics & Ground Check")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("UI")]
    [SerializeField] private Slider jumpBar;

    // --- Private State ---
    private float moveDirection = 0f;
    private bool isFacingRight = true;
    private bool isCharging = false;
    private float jumpCharge = 0f;

    // --- NEW: Animation Component ---
    private Animator animator;

    private void Awake()
    {
        // Get the Animator component attached to this object
        animator = GetComponent<Animator>();
        
        // Safety check just in case you forgot to assign RB in inspector
        if (rb == null) rb = GetComponent<Rigidbody2D>(); 
    }

    // =================================================================================
    // INPUT HANDLING (Unity Events)
    // =================================================================================

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        bool pressed = value.isPressed;

        if (pressed && IsGrounded())
        {
            // Start charging
            isCharging = true;
            jumpCharge = 0f;
            Debug.Log("Started charging jump");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop moving when charging
        }
        else if (!pressed && isCharging)
        {
            // Release jump
            ReleaseJump();
        }
    }

    // =================================================================================
    // UNITY LIFECYCLE
    // =================================================================================

    private void Update()
    {
        // Charge jump while button is held and grounded
        if (isCharging && IsGrounded())
        {
            jumpCharge += chargeRate * Time.deltaTime;
            jumpCharge = Mathf.Clamp01(jumpCharge);

            if (jumpBar != null)
                jumpBar.value = jumpCharge;
        }

        Flip();
        
        // --- NEW: Update Animations every frame ---
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (!isCharging)
            HandleMovement();
    }

    // =================================================================================
    // MOVEMENT & PHYSICS
    // =================================================================================

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }

    private void ReleaseJump()
    {
        float jumpPower = Mathf.Lerp(minJumpPower, maxJumpPower, jumpCharge);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        Debug.Log($"Jumped! Power: {jumpPower:F2} (Charge: {jumpCharge:F2})");

        // Reset state
        isCharging = false;
        jumpCharge = 0f;
        if (jumpBar != null)
            jumpBar.value = 0f;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, groundLayer) ||
            Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && moveDirection < 0f) || (!isFacingRight && moveDirection > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    // --- NEW: Animation Logic Helper ---
    private void UpdateAnimations()
    {
        if (animator == null) return;

        // 1. Pass Speed
        // We use the absolute value of the Rigidbody's actual x velocity.
        // This ensures that if "isCharging" stops the player, the animation stops too.
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // 2. Pass Jumping State
        // If we are NOT grounded, we are jumping (or falling).
        animator.SetBool("IsJumping", !IsGrounded());
    }
}