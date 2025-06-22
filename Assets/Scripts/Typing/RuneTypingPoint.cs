using UnityEngine;

public abstract class RuneTypingPoint : MonoBehaviour
{
    public TypingUI typingUI;
    [Header("Typing Words & Timer")]
    public string[] wordsToType = { "celeste", "typing", "runic", "magic" };
    public float timeLimit = 5f;

    protected bool completed = false;

    protected virtual void Start() { }

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
        Destroy(gameObject); 
    }

    protected virtual void OnTypingFail()
    {
        PlayerStats.Instance.TakeDamage(1);
    }
}
