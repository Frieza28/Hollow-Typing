using UnityEngine;

public class RuneTypingReveal : RuneTypingPoint
{
    public GameObject platformToReveal;

    protected override void Start()
    {
        base.Start();
        if (platformToReveal != null)
            platformToReveal.SetActive(false);
    }

    protected override void OnTypingSuccess()
    {
        if (platformToReveal != null)
            platformToReveal.SetActive(true);
        base.OnTypingSuccess();
    }

    // (Opcional) Podes implementar OnTypingFail para castigar ou ignorar falha
}
