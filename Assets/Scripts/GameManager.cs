using UnityEngine;
using System.Collections; // Necessário para Coroutines

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform respawnPoint;
    public int lives = 3;
    public PlayerController player;

    private void Awake()
    {
        Instance = this;
    }

    public void LoseLifeAndRespawn()
    {
        lives--;
        if (lives <= 0)
        {
            // Chama fim de jogo com delay
            EndGameAfterDelay(5f);
        }
        else
        {
            player.transform.position = respawnPoint.position;
        }
    }

    public void EndGameAfterDelay(float delaySeconds)
    {
        StartCoroutine(EndGameCoroutine(delaySeconds));
    }

    private IEnumerator EndGameCoroutine(float delaySeconds)
    {
        yield return new WaitForSecondsRealtime(delaySeconds); // Usar Realtime para funcionar com Time.timeScale = 0
        EndScene();
    }

    void EndScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Encerra o play mode no Editor
#else
        Application.Quit(); // Encerra o jogo em build real
#endif
    }
}
