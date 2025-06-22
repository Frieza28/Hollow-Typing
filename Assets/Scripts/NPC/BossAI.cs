using UnityEngine;
using System.Collections;
using TMPro;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackDistance = 25f;    
    public float fireRate = 0.35f;        
    public float projectileSpeed = 12f;
    public float moveSpeed = 6f;          
    public float followHeight = 3f;       

    private float fireCooldown = 0f;
    private Rigidbody2D rb;

    public float maxHealth = 3;
    private float currentHealth;

    public GameObject hiddenIslandGroup;
    public GameObject messagePanel;

    public BossHealthBar bossHealthBar;

    void Start()
    {
        fireCooldown = 0f;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + new Vector3(0, followHeight, 0);
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-20, 20, 20);
        else
            transform.localScale = new Vector3(20, 20, 20);

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        fireCooldown -= Time.deltaTime;
        if (distanceToPlayer <= attackDistance && fireCooldown <= 0f)
        {
            Attack();
            fireCooldown = fireRate;
        }
    }

    void Attack()
    {
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

            Rigidbody2D prb = projectile.GetComponent<Rigidbody2D>();
            if (prb != null)
                prb.linearVelocity = dir * projectileSpeed;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        bossHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        if (hiddenIslandGroup != null)
            hiddenIslandGroup.SetActive(true);
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            messagePanel.GetComponentInChildren<TMP_Text>().text = "Encontra a última ilha!";
            StartCoroutine(HideMessageAfterSeconds(3f));
        }
        Destroy(gameObject); 
    }

    IEnumerator HideMessageAfterSeconds(float secs)
    {
        yield return new WaitForSeconds(secs);
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
