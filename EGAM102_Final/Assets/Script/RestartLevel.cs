using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f; // Unpause the game just in case
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

