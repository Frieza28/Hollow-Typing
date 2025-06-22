using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogPanel;       
    public TextMeshProUGUI dialogText;   
    [TextArea] public string[] messages;

    private int currentMsg = 0;
    private bool playerNearby = false;
    private bool dialogActive = false;
    public GameObject interactHint;

    void Start()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    void Update()
    {
        if (interactHint != null)
            interactHint.SetActive(playerNearby && !dialogActive);

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogActive)
            {
                dialogActive = true;
                dialogPanel.SetActive(true);
                currentMsg = 0;
                dialogText.text = messages.Length > 0 ? messages[currentMsg] : "Olá!";
            }
            else
            {
                currentMsg++;
                if (currentMsg < messages.Length)
                {
                    dialogText.text = messages[currentMsg];
                }
                else
                {
                    dialogPanel.SetActive(false);
                    dialogActive = false;
                }
            }
        }
        if (!playerNearby && dialogPanel.activeSelf)
        {
            dialogPanel.SetActive(false);
            dialogActive = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
