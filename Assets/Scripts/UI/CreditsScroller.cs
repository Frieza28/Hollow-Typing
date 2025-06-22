using UnityEngine;
using TMPro;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsText;   
    public float scrollSpeed = 50f;    
    private float startY;

    void Start()
    {
        startY = creditsText.anchoredPosition.y;
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}
