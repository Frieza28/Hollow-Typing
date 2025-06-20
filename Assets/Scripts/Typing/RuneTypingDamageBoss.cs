using UnityEngine;

public class RuneTypingDamageBoss : RuneTypingPoint
{
    public GameObject bossObject;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTypingStart()
    {
    }

    protected override void OnTypingSuccess()
    {
        if (bossObject != null)
        {
            var bossAI = bossObject.GetComponent<BossAI>();
            if (bossAI != null)
            {
                bossAI.TakeDamage(1); 
            }
        }
        base.OnTypingSuccess(); 
    }
}
