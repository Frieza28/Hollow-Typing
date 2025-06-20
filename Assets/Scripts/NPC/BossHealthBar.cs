using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image fillImage; 
    private float maxHealth;
    
    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        SetHealth(value);
    }

    public void SetHealth(float value)
    {
        float pct = value / maxHealth;
        fillImage.fillAmount = pct;
    }
}
