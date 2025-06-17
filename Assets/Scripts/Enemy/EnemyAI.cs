using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2.5f;
    public float chaseDistance = 7f;
    public float attackDistance = 1.2f; // Distância para atacar
    public bool isChasing = false;

    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        isChasing = false;
    }

    void Update()
    {
        if (isDead) return;

        if (isChasing && player != null)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance <= attackDistance)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    if (animator != null) animator.SetTrigger("Attack");
                    // Dano no player
                    PlayerStats ps = player.GetComponent<PlayerStats>();
                    if (ps != null) ps.TakeDamage(1);
                    // Opcional: cooldown de ataque ou travar movimento
                    Invoke(nameof(ResetAttack), 1f); // 1 segundo entre ataques (ajusta ao tempo da animação)
                }
                // Não avança enquanto ataca
                if (animator != null) animator.SetBool("isMoving", false);
            }
            else
            {
                // Só persegue se não estiver a atacar
                if (!isAttacking)
                {
                    if (animator != null) animator.SetBool("isMoving", true);
                    Vector2 direction = (player.position - transform.position).normalized;
                    transform.position += (Vector3)direction * speed * Time.deltaTime;
                }
            }
        }
        else
        {
            if (animator != null) animator.SetBool("isMoving", false);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void ActivateChase()
    {
        Debug.Log($"{name} -- ActivateChase chamado! Frame: {Time.frameCount}");
        isChasing = true;
    }


    public void Die()
    {
        if (isDead) return;
        isDead = true;
        isChasing = false;
        if (animator != null) animator.SetTrigger("Death");
        // Destrói o inimigo após a animação de morte (ajusta ao tempo real da tua animação Death)
        Destroy(gameObject, 2.8f); // 1.2 segundos, muda se necessário
    }
}
