using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	// =================================================================================
	// INSPECTOR VARIABLES
	// =================================================================================

	[Header("Movement")]
	[SerializeField] private float speed = 8f;

	[Header("Charged Jump")]
	[SerializeField] private float maxJumpForce = 20f; // maximum jump
	[SerializeField] private float chargeRate = 0.5f;  // jump charge per frame

	[Header("Physics & Ground Check")]
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Transform groundCheckLeft;
	[SerializeField] private Transform groundCheckRight;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float rayLength = 0.2f;
	[SerializeField] private PhysicsMaterial2D normalMat;
	[SerializeField] private PhysicsMaterial2D bounceMat;

	[SerializeField] private float yDamping = 1f; // how quickly vertical speed slows

	[Header("UI")]
	[SerializeField] private Slider jumpBar;

	// =================================================================================
	// PRIVATE STATE
	// =================================================================================

	private float moveDirection = 0f;
	private bool isFacingRight = true;
	private bool isCharging = false;
	private float jumpCharge = 0f;
	private bool grounded;
	private bool wasGrounded;
	private bool preJump;

	private Animator animator;

	// =================================================================================
	// UNITY LIFECYCLE
	// =================================================================================

	private void Awake()
	{
		animator = GetComponent<Animator>();
		if (rb == null) rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// Update grounded status
		grounded = CheckGrounded();

		HandleJumpCharge();
		HandleLanding();
		Flip();
		UpdateAnimations();

		wasGrounded = grounded;
	}

	private void FixedUpdate()
	{
		HandleMovement();
		ApplyVerticalDamping();
	}

	// =================================================================================
	// INPUT
	// =================================================================================

	public void OnMove(InputValue value)
	{
		moveDirection = value.Get<Vector2>().x;
	}

	public void OnJump(InputValue value)
	{
		bool pressed = value.isPressed;

		if (pressed && grounded)
		{
			StartJumpCharge();
		}
		else if (!pressed && isCharging)
		{
			ReleaseJump();
		}
	}

	// =================================================================================
	// MOVEMENT
	// =================================================================================

	private void ApplyVerticalDamping()
	{
		// Reduce only the vertical velocity
		float newY = rb.linearVelocity.y * (1f - yDamping * Time.fixedDeltaTime);
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, newY);
	}

	private void HandleMovement()
	{
		if (!isCharging && grounded)
		{
			rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
		}
		else if (isCharging)
		{
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // lock horizontal while charging
		}
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

	// =================================================================================
	// JUMP
	// =================================================================================

	private void StartJumpCharge()
	{
		isCharging = true;
		jumpCharge = 0f;
		rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // stop horizontal
		rb.sharedMaterial = bounceMat;                // bounce material
		preJump = true;
	}

	private void HandleJumpCharge()
	{
		if (jumpCharge >= maxJumpForce)
		{
			ReleaseJump();
			return;
		}

		if (isCharging && grounded)
		{
			jumpCharge += chargeRate * Time.deltaTime;
			jumpCharge = Mathf.Clamp(jumpCharge, 0f, maxJumpForce);

			if (jumpBar != null)
				jumpBar.value = jumpCharge / maxJumpForce;
		}

		// Reset material when falling
		if (rb.linearVelocity.y < -0.1f)
		{
			rb.sharedMaterial = normalMat;
		}
		else 
			rb.sharedMaterial = bounceMat;
	}

	private void ReleaseJump()
	{
		float finalJump = jumpCharge;
		rb.linearVelocity = new Vector2(moveDirection * speed, finalJump);

		// Reset state
		isCharging = false;
		jumpCharge = 0f;
		preJump = false;
		if (jumpBar != null)
			jumpBar.value = 0f;

		rb.sharedMaterial = normalMat;
	}

	private void HandleLanding()
	{
		if (grounded && !wasGrounded)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
		}
	}

	// =================================================================================
	// GROUND CHECK
	// =================================================================================

	private bool CheckGrounded()
	{
		RaycastHit2D leftHit = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, rayLength, groundLayer);
		RaycastHit2D rightHit = Physics2D.Raycast(groundCheckRight.position, Vector2.down, rayLength, groundLayer);

		// Draw debug rays
		//Debug.DrawRay(groundCheckLeft.position, Vector2.down * rayLength, leftHit ? Color.green : Color.red);
		//Debug.DrawRay(groundCheckRight.position, Vector2.down * rayLength, rightHit ? Color.green : Color.red);

		return leftHit || rightHit;
	}

	// =================================================================================
	// ANIMATIONS
	// =================================================================================

	private void UpdateAnimations()
	{
		if (animator == null) return;

		animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
		animator.SetFloat("vSpeed", rb.linearVelocity.y);
		animator.SetBool("grounded", grounded);
		animator.SetBool("preJump", preJump);
	}
}
