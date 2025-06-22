using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public AudioClip levelCompletedClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelTimer timer = FindFirstObjectByType<LevelTimer>();
            if (timer != null)
                timer.StopTimer();

            levelCompletePanel.SetActive(true);

            var bgMusic = FindObjectOfType<LevelMusicManager>();
            if (bgMusic != null)
                bgMusic.StopMusic();

            if (audioSource != null && levelCompletedClip != null)
            {
                audioSource.PlayOneShot(levelCompletedClip);
                Invoke("GoToMenu", levelCompletedClip.length);
            }
            else
            {
                Invoke("GoToMenu", 2f); 
            }
        }
    }


    void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
