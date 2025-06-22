using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Animator animator;
    private bool exploded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Vector3 pos = transform.position;
        pos.z = -5;
        transform.position = pos;

        Destroy(gameObject, 10f); 
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;
    
        if (other.CompareTag("Player"))
        {
            PlayerStats ps = other.GetComponent<PlayerStats>();
            if (ps != null)
                ps.TakeDamage(1); 
    
            exploded = true;
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            var collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;
            if (animator != null)
                animator.SetTrigger("Death");
            Destroy(gameObject, 0.6f);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            exploded = true;
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            var collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;
            if (animator != null)
                animator.SetTrigger("Death");
            Destroy(gameObject, 0.6f);
        }
    }

}
