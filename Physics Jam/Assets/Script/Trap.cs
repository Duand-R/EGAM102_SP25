using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Restart the scene when the player collides with the trap.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
