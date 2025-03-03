using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI elements")]
    public Button restartButton;
    public Button fullscreenButton;
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private void Start()
    {
        // Set button event listening
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (fullscreenButton != null)
        {
            fullscreenButton.onClick.AddListener(ToggleFullscreen);
        }

        // Initial hiding of results panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}