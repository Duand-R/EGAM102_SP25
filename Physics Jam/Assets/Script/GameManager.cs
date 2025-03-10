using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float fallThreshold = -10f;

    void Update()
    {
        if (player.transform.position.y < fallThreshold)
        {
            // Restart the current scene if player falls too low
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
