using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (other.CompareTag("Pushable"))
        {
            RespawnablePickup pickup = other.GetComponent<RespawnablePickup>();
            if (pickup != null)
            {
                pickup.Respawn();
            }
            else
            {
                Destroy(other.gameObject); // fallback
            }
        }
    }
}
