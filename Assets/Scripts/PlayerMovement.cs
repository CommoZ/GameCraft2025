using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // --- Tunable Parameters (Visible in Inspector) ---
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;

    [Header("Physics & Ground Check")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    // --- Private State Variables ---
    private float moveDirection; // Represents the horizontal input (-1 to 1)
    private bool isFacingRight = true;
    private bool jumpRequested; // Flag to ensure jump occurs in FixedUpdate

    // NOTE: playerInput is automatically referenced by Unity's 'Invoke Unity Events' behavior,
    // so the private field and Awake() are often not strictly necessary unless you need 
    // to access the PlayerInput component specifically for other reasons.
    // private PlayerInput playerInput; 

    // void Awake()
    // {
    //     playerInput = GetComponent<PlayerInput>();
    // }

    // =================================================================================
    // INPUT HANDLING (Called by New Input System Events)
    // =================================================================================

    public void OnMove(InputValue value)
    {
        // Reads the Vector2 input (e.g., from WASD or Left Stick)
        Vector2 movement = value.Get<Vector2>();
        moveDirection = movement.x;
    }

    public void OnJump(InputValue value)
    {
        // 1. Check for Button Press (Start)
        if (value.isPressed)
        {
            // Set the flag to true. The actual jump physics is applied in FixedUpdate()
            // to ensure it is synchronized with the physics engine.
            Debug.Log("Jump button pressed");
            jumpRequested = true;
        }

        // 2. Handle Button Release (Cancel - For short hop/variable jump height)
        else // if (!value.isPressed)
        {
            // Only cut velocity if the player is currently moving upwards
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    // =================================================================================
    // UNITY LIFECYCLE METHODS
    // =================================================================================

    void Update()
    {
        // Update is ideal for non-physics-related tasks like setting animation states
        // and flipping the sprite, which need to be responsive.
        Flip();
    }

    private void FixedUpdate()
    {
        // FixedUpdate is where all Rigidbody/Physics calculations should happen.

        // 1. Horizontal Movement
        HandleMovement();

        // 2. Jumping (Triggered by the flag set in OnJump)
        HandleJump();
    }

    // =================================================================================
    // MOVEMENT & PHYSICS LOGIC
    // =================================================================================

    private void HandleMovement()
    {
        // Apply horizontal velocity based on input direction
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        // Debug.Log("Is grounded: " + IsGrounded());
        // Check the jump request flag AND if the player is currently grounded
        if (jumpRequested && IsGrounded())
        {
            // Apply the jump force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);

            Debug.Log("Jump executed");
            // Immediately clear the flag so we don't jump again next frame
            jumpRequested = false;
        }

        // Always clear the flag here if the player has left the ground, 
        // preventing a jump from being stored indefinitely.
        if (jumpRequested && !IsGrounded())
        {
            jumpRequested = false;
        }
    }
    private bool IsGrounded()
    {
        // Use the dedicated radius variable
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        // Only flip if there is horizontal input that conflicts with the current facing direction
        if ((isFacingRight && moveDirection < 0f) || (!isFacingRight && moveDirection > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}