using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBarController : MonoBehaviour
{
    public Transform player;
    public Transform startPoint;
    public Transform endPoint;

    public RectTransform barLeft;
    public RectTransform barRight;
    public RectTransform playerIcon;
    public Image progressBarFill;

    private Vector2 barLeftPos;
    private Vector2 barRightPos;

    void Start()
    {
        barLeftPos = barLeft.anchoredPosition;
        barRightPos = barRight.anchoredPosition;
    }

    void Update()
    {
        float playerX = player.position.x;
        float startX = startPoint.position.x;
        float endX = endPoint.position.x;

        float progress = Mathf.InverseLerp(startX, endX, playerX);
        progress = Mathf.Clamp01(progress);

        // Interpola entre a posição do barLeft e barRight (em UI)
        Vector2 iconPos = Vector2.Lerp(barLeftPos, barRightPos, progress);
        playerIcon.anchoredPosition = new Vector2(iconPos.x, playerIcon.anchoredPosition.y);

        if (progressBarFill != null)
            progressBarFill.fillAmount = progress;
    }
}
