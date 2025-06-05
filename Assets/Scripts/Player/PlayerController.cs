using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping = false;
    private bool isGrounded = false;
    private Animator animator;
    private Vector3 lastScale = new Vector3(5, 5, 5);

    // WALL SLIDE/GRAB
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.2f;
    public float wallGrabDuration = 5f;
    private float wallGrabTimer;
    private bool isTouchingWall = false;
    private bool isWallGrabbing = false;

    // COOLDOWN
    private float wallGrabCooldown = 5f;
    private float wallGrabCooldownTimer = 0f;
    private bool isInWallGrabCooldown = false;

    // WALLCHECK (Empty colocado do lado do personagem)
    public Transform wallCheck;
    public float wallCheckRadius = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isWallGrabbing)
        {
            // Salta para fora da parede: impulso contrário à parede e para cima
            isWallGrabbing = false;
            isInWallGrabCooldown = true;
            wallGrabCooldownTimer = wallGrabCooldown;
    
            // Deteta para que lado está encostado e aplica impulso contrário
            float jumpDirection = moveInput.x > 0 ? -1 : 1;
            rb.linearVelocity = new Vector2(jumpDirection * speed, jumpForce);
    
            animator.SetTrigger("JumpTrigger");
            Debug.Log("Saltou da parede!");
        }
        else if (context.started && isGrounded)
        {
            isJumping = true;
            animator.SetTrigger("JumpTrigger");
        }
    }


    private void Update()
    {
        // Flip sprite
        if (moveInput.x > 0.1f)
            lastScale = new Vector3(5, 5, 5);
        else if (moveInput.x < -0.1f)
            lastScale = new Vector3(-5, 5, 5);
        transform.localScale = lastScale;

        // Detecta parede (sempre do lado do WallCheck)
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        bool holdingDirection = Mathf.Abs(moveInput.x) > 0.1f;
        bool falling = rb.linearVelocity.y < -0.1f;

        // COOLDOWN TIMER
        if (isInWallGrabCooldown)
        {
            wallGrabCooldownTimer -= Time.deltaTime;
            if (wallGrabCooldownTimer <= 0)
            {
                isInWallGrabCooldown = false;
                wallGrabCooldownTimer = 0;
            }
        }

        // WALL GRAB LOGIC
        if (!isInWallGrabCooldown && isTouchingWall && !isGrounded && holdingDirection && falling)
        {
            if (!isWallGrabbing)
            {
                isWallGrabbing = true;
                wallGrabTimer = wallGrabDuration;
                Debug.Log("Começou Wall Grab!");
            }
            else
            {
                wallGrabTimer -= Time.deltaTime;
                Debug.Log($"Timer: {wallGrabTimer}");
                if (wallGrabTimer <= 0f)
                {
                    isWallGrabbing = false;
                    isInWallGrabCooldown = true;
                    wallGrabCooldownTimer = wallGrabCooldown;
                    Debug.Log("Largou a parede e iniciou cooldown!");
                }
            }
        }
        else if (!isWallGrabbing)
        {
            // Só desativa o wall grab se não está a segurar a parede (evita bug visual)
            isWallGrabbing = false;
        }

        // UI Pie: SÓ ativa durante wall grab (enquanto agarrado à parede)
        WallGrabUI.Instance?.SetClock(isWallGrabbing ? wallGrabTimer / wallGrabDuration : 0, isWallGrabbing);

        // ANIMAÇÕES:
        animator.SetBool("isWalking", Mathf.Abs(moveInput.x) > 0.1f && isGrounded);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
        animator.SetBool("isWallSliding", isWallGrabbing);
    }

    private void FixedUpdate()
    {
        // Movimento normal
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = false;
        }

        // Limitar descida durante wall grab
        if (isWallGrabbing)
        {
            float verticalInput = moveInput.y;
            rb.linearVelocity = new Vector2(0, Mathf.Clamp(verticalInput, -1, 1) * 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
