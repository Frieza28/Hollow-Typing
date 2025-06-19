using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;   // Prefab do projétil (com Rigidbody2D)
    public Transform firePoint;           // Onde o projétil nasce
    public float attackDistance = 8f;
    public float fireRate = 1.5f;         // Segundos entre tiros
    public float projectileSpeed = 8f;

    private float fireCooldown = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        fireCooldown = 0f;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= attackDistance)
        {
            // Olha para o player (flip horizontal)
            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-3, 3, 3);
            else
                transform.localScale = new Vector3(3, 3, 3);

            // Ataca se cooldown terminou
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Attack();
                fireCooldown = fireRate;
            }
        }
        // Não precisas de SetBool("isAttacking", false), pois não existe esse parâmetro!
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack"); // Usa o nome exato do trigger da tua animação de disparo ("Attack" no teu Animator)

        if (projectilePrefab != null && firePoint != null && player != null)
        {
            Vector3 firePos = firePoint.position;
            firePos.z = 0; // Força o Z=0 para garantir que aparece

            GameObject projectile = Instantiate(projectilePrefab, firePos, Quaternion.identity);
            Vector2 dir = (player.position - firePoint.position).normalized;

            // ----------- FLIP X (recomendado para sprites 2D) -----------
            float originalY = projectile.transform.localScale.y;
            float originalZ = projectile.transform.localScale.z;
            float originalX = Mathf.Abs(projectile.transform.localScale.x); // valor positivo original

            if (dir.x < 0)
                projectile.transform.localScale = new Vector3(-originalX, originalY, originalZ); // esquerda
            else
                projectile.transform.localScale = new Vector3(originalX, originalY, originalZ);  // direita

            // ------------------------------------------------------------

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * projectileSpeed; // ajusta a velocidade se quiseres
        }
    }

}
