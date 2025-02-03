using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        Debug.Log("StartGame called");
        SceneManager.LoadScene("MainScene");
    }

}
