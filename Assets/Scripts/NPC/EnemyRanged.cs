using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;  
    public Transform firePoint;           
    public float attackDistance = 8f;
    public float fireRate = 1.5f;         
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
            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-3, 3, 3);
            else
                transform.localScale = new Vector3(3, 3, 3);

            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Attack();
                fireCooldown = fireRate;
            }
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack"); 

        if (projectilePrefab != null && firePoint != null && player != null)
        {
            Vector3 firePos = firePoint.position;
            firePos.z = 0; 

            GameObject projectile = Instantiate(projectilePrefab, firePos, Quaternion.identity);
            Vector2 dir = (player.position - firePoint.position).normalized;

            float originalY = projectile.transform.localScale.y;
            float originalZ = projectile.transform.localScale.z;
            float originalX = Mathf.Abs(projectile.transform.localScale.x); 

            if (dir.x < 0)
                projectile.transform.localScale = new Vector3(-originalX, originalY, originalZ); 
            else
                projectile.transform.localScale = new Vector3(originalX, originalY, originalZ); 

            // ------------------------------------------------------------

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * projectileSpeed; 
        }
    }

}
