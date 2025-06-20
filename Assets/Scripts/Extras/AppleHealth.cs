using UnityEngine;

public class AppleHealth : MonoBehaviour
{
    public int healthRestore = 1; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Heal(healthRestore);
            }

            Destroy(gameObject); 
        }
    }
}
