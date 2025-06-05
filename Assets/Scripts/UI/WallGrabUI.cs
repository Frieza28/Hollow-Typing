using UnityEngine;
using UnityEngine.UI;

public class WallGrabUI : MonoBehaviour
{
    public static WallGrabUI Instance;
    public Image pieImage;

    private void Awake()
    {
        Instance = this;
        // Inicialmente esconde o pie
        pieImage.gameObject.SetActive(false);
    }

    public void SetClock(float fillAmount, bool show)
    {
        if (pieImage == null) return;
    
        pieImage.gameObject.SetActive(show); // Só ativa quando show==true
        pieImage.fillAmount = Mathf.Clamp01(fillAmount);
    }


}
