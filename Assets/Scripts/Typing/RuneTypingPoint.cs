using UnityEngine;

public class RuneTypingPoint : MonoBehaviour
{
    public TypingUI typingUI;
    public GameObject platformToRotate;
    [Header("Transformações (Inspector)")]
    public Vector3 finalPosition;
    public Vector3 finalRotation;
    public Vector3 initialPosition;
    public Vector3 initialRotation;


    [Header("Typing Words & Timer")]
    public string[] wordsToType = { "celeste", "typing", "runic", "magic" };
    public float timeLimit = 5f;

    private bool completed = false;

    private void Start()
    {
        // Garante que as posições iniciais estão corretas no arranque do jogo
        if (platformToRotate != null)
        {
            platformToRotate.transform.position = initialPosition;
            platformToRotate.transform.eulerAngles = initialRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!completed && other.CompareTag("Player"))
        {
            typingUI.StartTyping(wordsToType, timeLimit, OnTypingSuccess, OnTypingFail);
        }
    }

    private void OnTypingSuccess()
    {
        if (platformToRotate != null)
        {
            platformToRotate.transform.position = finalPosition;
            platformToRotate.transform.eulerAngles = finalRotation;
        }
        completed = true;
        Destroy(gameObject); // Remove a runa do mapa depois de usada
    }

    private void OnTypingFail()
    {
        if (platformToRotate != null)
        {
            platformToRotate.transform.position = initialPosition;
            platformToRotate.transform.eulerAngles = initialRotation;
        }
        // **Tira uma vida**
        PlayerStats.Instance.TakeDamage(1);
        // Podes meter lógica para "voltar ao início do nível" aqui.
    }
}
