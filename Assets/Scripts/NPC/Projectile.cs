using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Animator animator;
    private bool exploded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Força Z=0 sempre que nasce
        Vector3 pos = transform.position;
        pos.z = -5;
        transform.position = pos;

        Destroy(gameObject, 10f); // tempo máximo de vida
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;
    
        if (other.CompareTag("Player"))
        {
            // Tira 1 coração/vida ao jogador
            PlayerStats ps = other.GetComponent<PlayerStats>();
            if (ps != null)
                ps.TakeDamage(1); // ou o método que usas para perder vida
    
            exploded = true;
            // Parar movimento e colisão, animar explosão, destruir, etc.
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            var collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;
            if (animator != null)
                animator.SetTrigger("Death");
            Destroy(gameObject, 0.6f);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
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
