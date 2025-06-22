using UnityEngine;
using System.Collections; 

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
        yield return new WaitForSecondsRealtime(delaySeconds); 
        EndScene();
    }

    void EndScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); 
#endif
    }
}
