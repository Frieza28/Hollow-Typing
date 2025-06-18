using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    [Space]
    [Tooltip("Player Movement Speed")]
    [Range(1f, 10f)]
    [SerializeField]
    private float speed = 6f;
    [Tooltip("Player Jump Force")]
    [Range(1f, 20f)]
    [SerializeField]
    private float jumpForce = 7f;

    // TODO: Refactor wall grab settings into a separate class or scriptable object for better organization and reusability.
    [Header("Wall Grab Settings")]
    [Space]
    [Tooltip("Layer mask for walls that can be grabbed")]
    [SerializeField]
    private LayerMask wallLayer;
    [Tooltip("Distance to check for walls when grabbing")]
    [Range(0.1f, 1f)]
    [SerializeField]
    private float wallCheckDistance = 0.2f;
    [Tooltip("Duration of wall grab before cooldown starts")]
    [Range(1f, 10f)]
    [SerializeField]
    private float wallGrabDuration = 5f;
    [Tooltip("Cooldown time after releasing wall grab")]
    [Range(1f, 10f)]
    [SerializeField]
    private float wallGrabCooldown = 5f;
    [Space(10)]
    [Tooltip("Transform to check for walls")]
    [SerializeField]
    private Transform wallCheck;
    [Tooltip("Radius for wall detection")]
    [Range(0.1f, 1f)]
    [SerializeField]
    private float wallCheckRadius = 0.2f;

    private float wallGrabCooldownTimer = 0f;
    private bool isInWallGrabCooldown = false;
    private float wallGrabTimer;
    private bool isTouchingWall = false;
    private bool isWallGrabbing = false;
    private int wallSide = 0; // -1 left, 1 right

    private bool isOnLadder = false;
    // TODO: Not using atm
    private float ladderSpeed = 4f;
    public Transform startPoint; // TODO:?  arrasta aqui o StartPoint no inspector

    // State variables
    private Vector2 moveInput;
    private bool isJumping = false;
    private bool isGrounded = false;
    private Vector3 lastScale = new(5, 5, 5);
    private Rigidbody2D rb;
    private Animator animator;
    private GroundSensor groundSensor;

    public void TakeDamage(int dmg)
    {
        // TODO: subtract health
        // TODO: move to animate class
        animator.SetTrigger("hurt");

        //if (health <= 0)
        //{
        //      Die();
        //    anim.PlayDie();
        //    // Disable input, collider, etc.
        //}
    }

    private void Die()
    {
        // TODO: Implement death logic, like respawning or game over
        // TODO: move to animate class
        animator.SetTrigger("die");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundSensor = GetComponentInChildren<GroundSensor>();
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

            animator.SetTrigger("jump");
            Debug.Log("Saltou da parede!");
        }
        else if (context.started && groundSensor.CanJump())
        {
            isJumping = true;
            animator.SetTrigger("jump");
        }
    }

    private void Update()
    {
        // TODO: Refactor animator parameters into a separate class or scriptable object for better organization and reusability.
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
        // TODO: Refactor animator parameters into a separate class or scriptable object for better organization and reusability.
        animator.SetFloat("horizontal", Mathf.Abs(moveInput.x));
        animator.SetFloat("vertical", rb.linearVelocity.y);
        animator.SetBool("isGrounded", groundSensor.IsGrounded);
        animator.SetBool("isWallSliding", isWallGrabbing);

        // TODO: Use Kinematic Rigidbody2D instead?
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
        if (transform.position.y <= -20f)
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
