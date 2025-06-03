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
    private int wallSide = 0; // -1 esquerda, 1 direita

    private bool isOnLadder = false;
    private float ladderSpeed = 4f;
    public Transform startPoint; // arrasta aqui o StartPoint no inspector

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
            isWallGrabbing = false;
            isInWallGrabCooldown = true;
            wallGrabCooldownTimer = wallGrabCooldown;

            // Impulso para fora da parede
            float jumpDir = wallSide != 0 ? -wallSide : (lastScale.x > 0 ? -1f : 1f);

            // Impulso lateral forte e ligeiro "nudge" para longe da parede
            rb.linearVelocity = new Vector2(jumpDir * speed * 1.2f, jumpForce);
            transform.position += new Vector3(jumpDir * 0.08f, 0, 0);

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

        // WallSide: guarda o último lado encostado
        if (isTouchingWall)
        {
            wallSide = lastScale.x > 0 ? 1 : -1;
        }

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
        animator.SetFloat("horizontal", Mathf.Abs(moveInput.x));
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
        animator.SetBool("isWallSliding", isWallGrabbing);

        // Movimento vertical na escada
        if (isOnLadder)
        {
            float verticalInput = moveInput.y;
            rb.gravityScale = 0; // Desativa gravidade enquanto está na escada
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * ladderSpeed);
        }
        else
        {
            rb.gravityScale = 1; // Volta à gravidade normal
        }

        // --- CHECK FALL/RESPAWN ---
        if (transform.position.y <= -5f)
        {
            // Perde 1 vida
            PlayerStats.Instance.TakeDamage(1);
    
            // Teleporta para o start point
            transform.position = startPoint.position;
    
            // Zera velocidade se quiseres
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isOnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isOnLadder = false;
        }
    }

}
