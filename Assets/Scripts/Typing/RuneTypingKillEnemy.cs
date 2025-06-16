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
        // Só ativa quando o Typing UI estiver ativo, só uma vez!
        if (!chaseActivated && typingUI != null && typingUI.isActiveAndEnabled)
        {
            if (enemyAI != null)
                enemyAI.ActivateChase();
            chaseActivated = true;
        }
    }

    protected override void OnTypingSuccess()
    {
        if (enemyToKill != null)
        {
            Destroy(enemyToKill);
        }
        base.OnTypingSuccess();
    }
}
