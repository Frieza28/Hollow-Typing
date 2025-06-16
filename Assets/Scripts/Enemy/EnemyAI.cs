using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2.5f;
    public bool isChasing = false;
    public float chaseDistance = 7f; // Distância máxima para perseguir

    void Update()
    {
        if (isChasing && player != null)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance <= chaseDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;
            }
            // Se quiseres que o inimigo pare de perseguir se o player fugir, podes usar:
            // else { isChasing = false; }
        }
    }

    public void ActivateChase()
    {
        isChasing = true;
    }

    // Detetar colisão com o player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isChasing && other.CompareTag("Player"))
        {
            // Perde uma vida e volta ao start point
            PlayerStats ps = other.GetComponent<PlayerStats>();
            if (ps != null) ps.TakeDamage(1);

            // Repor posição do jogador (assumindo que tens GameManager)
            GameManager.Instance.player.transform.position = GameManager.Instance.respawnPoint.position;

            // (Opcional) parar perseguição depois do ataque
            // isChasing = false;
        }
    }
}
