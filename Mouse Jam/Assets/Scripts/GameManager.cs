using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public float gameTime = 30f;
    public float spawnInterval = 1f;
    public float minDistanceFromEnemy = 3f;
    public AudioClip backgroundMusic;

    private int score = 0;
    private float timer;
    private bool isGameActive = false;
    private GameObject enemyAI;
    private AudioSource audioSource;


    void Start()
    {
        enemyAI = GameObject.FindWithTag("Enemy");
        if (enemyAI == null)
        {
            Debug.LogError("Enemy AI not found! Make sure there is an object with the 'Enemy' tag.");
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        StartGame();
    }

    void StartGame()
    {
        isGameActive = true;
        timer = gameTime;
        score = 0;
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
                CheckWinCondition();
            }
            UpdateUI();
        }

        if (!isGameActive && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    void SpawnTarget()
    {
        if (isGameActive && enemyAI != null)
        {
            Vector2 spawnPosition;
            int attempts = 0;

            do
            {
                spawnPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
                attempts++;
            } while (Vector2.Distance(spawnPosition, enemyAI.transform.position) < minDistanceFromEnemy && attempts < 10);

            if (attempts < 10)
            {
                Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn position for the target.");
            }
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

    void CheckWinCondition()
    {
        EnemyAI enemyAIComponent = enemyAI.GetComponent<EnemyAI>();
        if (enemyAIComponent != null && enemyAIComponent.GetCurrentHealth() >= 1)
        {
            GameWin();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        CancelInvoke("SpawnTarget");
        SceneManager.LoadScene("GameOverScene");
    }

    public void GameWin()
    {
        isGameActive = false;
        CancelInvoke("SpawnTarget");
        SceneManager.LoadScene("GameWinScene");
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time: " + Mathf.Round(timer);
    }
}