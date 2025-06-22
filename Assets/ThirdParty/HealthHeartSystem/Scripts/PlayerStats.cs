/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }
    public GameObject gameOverPanel;

    public AudioClip takeDamageClip;
    private AudioSource audioSource;
    public AudioClip gameOverClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;              
        if (health < 0) health = 0;    
    
        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    
        if (audioSource != null && takeDamageClip != null)
            audioSource.PlayOneShot(takeDamageClip);

        if (health == 0)
        {
            GameOver();
        }
    }


    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    private void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        
        var bgMusic = FindObjectOfType<LevelMusicManager>();
        if (bgMusic != null)
            bgMusic.StopMusic();

        if (audioSource != null && gameOverClip != null)
        {
            audioSource.PlayOneShot(gameOverClip);
            StartCoroutine(WaitAndGoToMenu(gameOverClip.length));
        }
        else
        {
            StartCoroutine(WaitAndGoToMenu(2.5f));
        }
    }

    private IEnumerator WaitAndGoToMenu(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth); 
        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }


}
