using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChestCollectible : MonoBehaviour
{
    public GameObject chestIconHUD; 
    public Image chestIconImage;

    private void Start()
    {
        // Começa invisível
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
                chestIconHUD.SetActive(true); // Mostra o ícone no HUD
                Color c = chestIconImage.color;
                c.a = 1f;   // 1f = 255 (totalmente opaco)
                chestIconImage.color = c;
            }
            Destroy(gameObject); // Remove o baú do mapa
        }
    }


}
