using UnityEngine;

public class RuneTypingKillEnemy : RuneTypingPoint
{
    public GameObject enemyToControl; // Inimigo a controlar

    private EnemyAI enemyAI;

    protected override void Start()
    {
        base.Start();
        if (enemyToControl != null)
            enemyAI = enemyToControl.GetComponent<EnemyAI>(); // Usa o nome do script do teu inimigo!
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyAI != null)
        {
            enemyAI.ActivateChase(); // O inimigo começa a perseguir o jogador
        }
    }

    protected override void OnTypingSuccess()
    {
        if (enemyToControl != null)
        {
            // Se tiveres método de morte, chama-o. Senão, usa Destroy:
            Destroy(enemyToControl);
        }
        base.OnTypingSuccess();
    }
}
