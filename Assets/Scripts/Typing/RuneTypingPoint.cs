using UnityEngine;

public abstract class RuneTypingPoint : MonoBehaviour
{
    public TypingUI typingUI;
    [Header("Typing Words & Timer")]
    public string[] wordsToType = { "celeste", "typing", "runic", "magic" };
    public float timeLimit = 5f;

    protected bool completed = false;

    // Subclasses podem precisar de Start para setar estado inicial
    protected virtual void Start() { }

    // Na classe base
    protected virtual void OnTypingStart() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!completed && other.CompareTag("Player"))
        {
            OnTypingStart();
            typingUI.StartTyping(wordsToType, timeLimit, OnTypingSuccess, OnTypingFail);
        }
    }

    protected virtual void OnTypingSuccess()
    {
        completed = true;
        Destroy(gameObject); // Remove a runa do mapa depois de usada
    }

    protected virtual void OnTypingFail()
    {
        // Perde vida por defeito
        PlayerStats.Instance.TakeDamage(1);
        // Podes meter lógica extra nas subclasses
    }
}
