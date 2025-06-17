using UnityEngine;

public class RuneTypingKillEnemy : RuneTypingPoint
{
    public GameObject enemyToKill;
    private EnemyAI enemyAI;
    private bool chaseActivated = false;

    protected override void Start()
    {
        base.Start();
        if (enemyToKill != null)
            enemyAI = enemyToKill.GetComponent<EnemyAI>();
    }

    void Update()
    {
    }

    protected override void OnTypingStart()
    {
        if (!chaseActivated && enemyAI != null)
        {
            enemyAI.ActivateChase();
            chaseActivated = true;
        }
    }


    protected override void OnTypingSuccess()
    {
        if (enemyToKill != null)
        {
            var enemyAI = enemyToKill.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.Die(); // Chama a animação de morte e destroi depois
            }
            else
            {
                Destroy(enemyToKill); // fallback
            }
        }
        base.OnTypingSuccess();
    }

}
