using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pedestrianPrefab;
    public Transform[] upperSpawnPoints;
    public Transform[] lowerSpawnPoints; 
    public Transform[] upperEndPoints; 
    public Transform[] lowerEndPoints;

    public float minSpawnTime = 2.0f;
    public float maxSpawnTime = 5.0f;

    public TextMeshProUGUI scoreText;
    public Button restartButton;


    public int maxLives = 3;
    private int currentLives;
    public GameObject[] playerIcons;
    public TextMeshProUGUI missText;

    private int score = 0;
    private bool isGameOver = false;

    void Start()
    {
        currentLives = maxLives;
        HideAllPlayerIcons();
        missText.gameObject.SetActive(false);
        UpdateScoreText();

        restartButton.onClick.AddListener(RestartGame);
        restartButton.gameObject.SetActive(false); 

        StartGame();
    }

    void HideAllPlayerIcons()
    {
        foreach (GameObject icon in playerIcons)
        {
            icon.SetActive(false);
        }
    }

    public void StartGame()
    {
        StartCoroutine(SpawnPedestrians());
    }

    IEnumerator SpawnPedestrians()
    {
        while (!isGameOver)
        {

            int pathType = Random.Range(0, 4);

            Transform spawnPoint, endPoint;
            bool isUpper = false;
            bool isLeftToRight = false;

            switch (pathType)
            {
                case 0: 
                    spawnPoint = upperSpawnPoints[0];
                    endPoint = upperEndPoints[0];
                    isUpper = true;
                    isLeftToRight = true;
                    break;
                case 1: 
                    spawnPoint = upperSpawnPoints[1];
                    endPoint = upperEndPoints[1];
                    isUpper = true;
                    isLeftToRight = false;
                    break;
                case 2: 
                    spawnPoint = lowerSpawnPoints[0];
                    endPoint = lowerEndPoints[0];
                    isUpper = false;
                    isLeftToRight = true;
                    break;
                default: 
                    spawnPoint = lowerSpawnPoints[1];
                    endPoint = lowerEndPoints[1];
                    isUpper = false;
                    isLeftToRight = false;
                    break;
            }

            
            GameObject pedestrian = Instantiate(pedestrianPrefab, spawnPoint.position, Quaternion.identity);
            PedestrianController controller = pedestrian.GetComponent<PedestrianController>();

            
            controller.moveDirection = isLeftToRight ?
                PedestrianController.MovementDirection.LeftToRight :
                PedestrianController.MovementDirection.RightToLeft;
            controller.isUpperLevel = isUpper;
            controller.startPosition = spawnPoint;
            controller.endPosition = endPoint;

            
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void AddScore()
    {
        score++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void LoseLife(Vector3 position)
    {
        
        currentLives--;

        
        if (missText != null)
        {
            missText.gameObject.SetActive(true);
            StartCoroutine(ShowMissText());
        }

        
        int missCount = maxLives - currentLives;
        if (missCount > 0 && missCount <= playerIcons.Length)
        {
            
            playerIcons[missCount - 1].SetActive(true);
        }

        
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    
    IEnumerator ShowMissText()
    {
        yield return new WaitForSeconds(1.5f);
        missText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        restartButton.gameObject.SetActive(true);

        
        ManholeController manholeController = FindAnyObjectByType<ManholeController>();
        if (manholeController != null)
        {
            manholeController.GameOver();
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void RestartGame()
    {
        score = 0;
        currentLives = maxLives;
        isGameOver = false;
        restartButton.gameObject.SetActive(false); 
        HideAllPlayerIcons();
        missText.gameObject.SetActive(false); 
        UpdateScoreText();

        
        PedestrianController[] pedestrians = FindObjectsByType<PedestrianController>(FindObjectsSortMode.None);
        foreach (PedestrianController ped in pedestrians)
        {
            Destroy(ped.gameObject);
        }

        
        ManholeController manholeController = FindAnyObjectByType<ManholeController>();
        if (manholeController != null)
        {
            manholeController.RestartGame();
        }

        
        StartCoroutine(SpawnPedestrians());
    }
}