using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private Vector2 boxSize = new(0.9f, 0.1f);
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    private float lastGroundedTime;

    private void FixedUpdate()
    {
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, groundMask);
        isGrounded = overlappingColliders.Length > 0;

        if (isGrounded) lastGroundedTime = Time.unscaledTime;
    }

    // Simple coyote check
    public bool CanJump(float coyoteWindow = 0.1f) =>
        IsGrounded || Time.time - lastGroundedTime <= coyoteWindow;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}

