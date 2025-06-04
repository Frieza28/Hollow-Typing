using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float jumpForce = 14f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping = false;
    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Precisa de ser exatamente assim para o Unity reconhecer!
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            isJumping = true;
        }
    }

    private Vector3 lastScale = new Vector3(5, 5, 5);

    private void Update()
    {
        if (moveInput.x > 0.1f)
            lastScale = new Vector3(5, 5, 5);
        else if (moveInput.x < -0.1f)
            lastScale = new Vector3(5, 5, 5);
    
        transform.localScale = lastScale;
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = false;
        }
    }

    // Simples deteção de chão
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
