using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public GameObject levelCompletePanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            levelCompletePanel.SetActive(true);
            Invoke("EndScene", 2f);
        }
    }

    void EndScene()
    {
#if UNITY_EDITOR
        // Só funciona no Editor!
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Build real
#endif
    }
}
