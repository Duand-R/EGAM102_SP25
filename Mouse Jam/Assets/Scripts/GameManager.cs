using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public float gameTime = 30f;
    public float spawnInterval = 1f;

    private int score = 0;
    private float timer;
    private bool isGameActive = false;

    void Start()
    {
        timer = gameTime;
        isGameActive = true;
        UpdateUI();
        InvokeRepeating("SpawnTarget", 0f, spawnInterval);
    }

    void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                isGameActive = false;
                Debug.Log("Game Over!");
            }
            UpdateUI();
        }
    }

    void SpawnTarget()
    {
        if (isGameActive)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
            Instantiate(targetPrefab, randomPosition, Quaternion.identity);
        }
    }

    public void IncreaseScore()
    {
        if (isGameActive)
        {
            score++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time: " + Mathf.Round(timer);
    }
}
