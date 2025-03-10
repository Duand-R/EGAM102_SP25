using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        // Loads Level1
        SceneManager.LoadScene("Level1");
    }
}

