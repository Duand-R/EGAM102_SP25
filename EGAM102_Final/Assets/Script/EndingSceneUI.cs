using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneUI : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Level1");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
