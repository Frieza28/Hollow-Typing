using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float totalTime = 60f; // tempo total do nível (segundos)
    private float timeRemaining;
    public PlayerStats playerStats;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endMessage;

    private bool timerActive = true;

    void Start()
    {
        timeRemaining = totalTime;
        endMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!timerActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0; // nunca deixa ir negativo
            UpdateTimerDisplay();
        }
        else
        {
            EndTime();
        }
    }


    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(Mathf.Max(timeRemaining, 0) / 60f);
        int seconds = Mathf.FloorToInt(Mathf.Max(timeRemaining, 0) % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }


    void EndTime()
    {
        timerActive = false;
        playerStats.TakeDamage((int)playerStats.Health);
        endMessage.text = "FIM DO TEMPO";
        endMessage.gameObject.SetActive(true);
    
        // Espera 5 segundos antes de sair
        StartCoroutine(WaitAndQuit());
    }
    
    private System.Collections.IEnumerator WaitAndQuit()
    {
        yield return new WaitForSecondsRealtime(5f); // Importante usar Realtime!
    
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
