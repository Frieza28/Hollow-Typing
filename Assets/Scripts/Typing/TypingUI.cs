using UnityEngine;
using TMPro;
using System.Collections;

public class TypingUI : MonoBehaviour
{
    public GameObject typingPanel;
    public TextMeshProUGUI wordText;
    public TMP_InputField inputField;
    public TextMeshProUGUI timerText;

    private string currentWord;
    private float timer;
    private Coroutine typingCoroutine;
    private System.Action onSuccess;
    private System.Action onFail;

    public void StartTyping(string[] words, float timeLimit, System.Action onSuccess, System.Action onFail)
    {
        var player = GameObject.FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().angularVelocity = 0;
            player.enabled = false; // Desativa o PlayerController!
        }

        typingPanel.SetActive(true);
        currentWord = words[Random.Range(0, words.Length)];
        wordText.text = currentWord;
        inputField.text = "";
        inputField.ActivateInputField();
        timer = timeLimit;
        this.onSuccess = onSuccess;
        this.onFail = onFail;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypingRoutine());
    }


    public void CloseTypingPanel()
    {
        typingPanel.SetActive(false);
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // Reativa o PlayerController SEMPRE ao fechar o painel
        var player = GameObject.FindObjectOfType<PlayerController>();
        if (player != null)
            player.enabled = true;
    }


    IEnumerator TypingRoutine()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
    
            if (inputField.text == currentWord)
            {
                Debug.Log("ACERTOU a palavra!");
                CloseTypingPanel();
                onSuccess?.Invoke();
                yield break;
            }
            yield return null;
        }
        Debug.Log("FALHOU a palavra ou tempo esgotado!");
        CloseTypingPanel();
        onFail?.Invoke();
    }

}
