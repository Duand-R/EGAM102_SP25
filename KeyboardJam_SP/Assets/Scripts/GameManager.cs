using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject arrowPrefab; // arrow prefab
    public Transform spawnArea; // arrow spawn area
    public float spawnInterval = 2f; // spawn time in between
    public float arrowLifetime = 5f; // arrow life time
    public TMP_Text scoreText; // UI for score text
    public TMP_Text livesText;

    private List<GameObject> activeArrows = new List<GameObject>(); //active arrow in the scene
    private int score = 0; // player score
    private int lives = 3;
    private int correctCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("arrowPrefab is not assigned!");
            return;
        }

        if (spawnArea == null)
        {
            Debug.LogError("spawnArea is not assigned!");
            return;
        }

        StartCoroutine(SpawnArrows());
        UpdateScoreText();
        UpdateLivesText();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
    }

    IEnumerator SpawnArrows()
    {
        while (true)
        {
            string[] directions = { "Up", "Down", "Left", "Right" };// make four direction 
            string randomDirection = directions[Random.Range(0, directions.Length)];// random choose one direction

            GameObject arrow = Instantiate(arrowPrefab, spawnArea.position, Quaternion.identity);// spawn arrow on spawnArea

            Arrow arrowComponent = arrow.GetComponent<Arrow>();// Get Arrow from object
            if (arrowComponent == null)
            {
                Debug.LogError("Arrow component is missing on the arrowPrefab!");
                yield break;
            }

            arrow.GetComponent<Arrow>().Initialize(randomDirection, arrowLifetime);
            activeArrows.Add(arrow);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void CheckPlayerInput()
    {
        if (activeArrows.Count > 0)
        {
            GameObject firstArrow = activeArrows[0];
            string arrowDirection = firstArrow.GetComponent<Arrow>().direction;

            if (Input.GetKeyDown(KeyCode.UpArrow) && arrowDirection == "Up" ||
               Input.GetKeyDown(KeyCode.DownArrow) && arrowDirection == "Down" ||
               Input.GetKeyDown(KeyCode.LeftArrow) && arrowDirection == "Left" ||
               Input.GetKeyDown(KeyCode.RightArrow) && arrowDirection == "Right")
            {
                Destroy(firstArrow);
                activeArrows.RemoveAt(0);
                score++;
                correctCount++;
                UpdateScoreText();

                if (correctCount >= 20)
                {
                    WinGame();
                }
            }
            else if (Input.anyKeyDown)
            {
                Debug.Log("Wrong Input!");
                LoseLife();
            }
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }

    void LoseLife()
    {
        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        StopAllCoroutines();
        foreach (var arrow in activeArrows)
        {
            Destroy(arrow);
        }
        activeArrows.Clear();
        SceneManager.LoadScene("GameOverScene");
    }

    void WinGame()
    {
        Debug.Log("You Win!");
        StopAllCoroutines();
        foreach (var arrow in activeArrows)
        {
            Destroy(arrow);
        }
        activeArrows.Clear();
        SceneManager.LoadScene("WinScene");
    }
}
