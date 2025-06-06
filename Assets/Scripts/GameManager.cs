using UnityEngine;

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
            // Fim de jogo, reinicia tudo ou mostra menu
        }
        else
        {
            player.transform.position = respawnPoint.position;
        }
    }
}
