using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Setting")]
    public int maxLives = 3;
    public float gameSpeed = 1.0f;
    public float speedIncreaseFactor = 0.1f;
    public float speedIncreaseInterval = 30.0f;

    [Header("UI reference")]
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private int currentScore = 0;
    private int currentLives;
    private bool isGameOver = false;
    private float timer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateUI();
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= speedIncreaseInterval)
        {
            timer = 0f;
            IncreaseGameSpeed();
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }

    public void LoseLife()
    {
        if (isGameOver)
        {
            return;
        }

        currentLives--;
        UpdateUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void Win()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        winPanel.SetActive(true);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }

        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    private void IncreaseGameSpeed()
    {
        gameSpeed += speedIncreaseFactor;
        Debug.Log("Game speed increased to: " + gameSpeed);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }
}