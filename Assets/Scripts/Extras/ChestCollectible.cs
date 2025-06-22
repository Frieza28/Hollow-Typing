using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChestCollectible : MonoBehaviour
{
    public GameObject chestIconHUD; 
    public Image chestIconImage;
    public AudioSource chestAudioSource;

    private void Start()
    {
        if (chestIconImage != null)
        {
            Color c = chestIconImage.color;
            c.a = 0f;
            chestIconImage.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (chestIconHUD != null && chestIconImage != null)
            {
                chestIconHUD.SetActive(true); 
                Color c = chestIconImage.color;
                c.a = 1f;   
                chestIconImage.color = c;
            }

            Destroy(gameObject); 
        }
    }


}
