using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    void Start()
    {
        GameObject menuMusic = GameObject.FindWithTag("MenuMusic");
        if (menuMusic != null)
        {
            Destroy(menuMusic);
        }
    }

    public void StopMusic() { GetComponent<AudioSource>().Stop(); }
}
