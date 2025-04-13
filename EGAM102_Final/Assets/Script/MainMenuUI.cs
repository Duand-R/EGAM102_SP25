using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject levelSelectPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        controlsPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
