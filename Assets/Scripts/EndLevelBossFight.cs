using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndLevelBossFight : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public AudioClip levelCompletedClip;
    private AudioSource audioSource;
    public ScreenFader screenFader; 

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

            StartCoroutine(CompleteAndFade());

            var bgMusic = FindFirstObjectByType<LevelMusicManager>();
            if (bgMusic != null)
                bgMusic.StopMusic();
        }
    }

    private IEnumerator CompleteAndFade()
    {
        if (audioSource != null && levelCompletedClip != null)
            audioSource.PlayOneShot(levelCompletedClip);

        yield return new WaitForSeconds(levelCompletedClip ? levelCompletedClip.length : 2f);

        if (screenFader != null)
            yield return screenFader.FadeOut();

        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
    }
}
