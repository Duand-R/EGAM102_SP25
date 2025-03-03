using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManholeController : MonoBehaviour
{
    public GameObject[] playerSprites; 
    private int currentPosition = 0; 

    private bool isGameOver = false;

    void Start()
    {
        
        UpdateActiveSprite();
    }

    void Update()
    {
        if (isGameOver) return;

        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentPosition == 2) currentPosition = 0;
            if (currentPosition == 3) currentPosition = 1;
            UpdateActiveSprite();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentPosition == 0) currentPosition = 2;
            if (currentPosition == 1) currentPosition = 3;
            UpdateActiveSprite();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPosition == 1) currentPosition = 0;
            if (currentPosition == 3) currentPosition = 2;
            UpdateActiveSprite();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPosition == 0) currentPosition = 1;
            if (currentPosition == 2) currentPosition = 3;
            UpdateActiveSprite();
        }
    }

    void UpdateActiveSprite()
    {
        
        for (int i = 0; i < playerSprites.Length; i++)
        {
            playerSprites[i].SetActive(i == currentPosition);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public void RestartGame()
    {
        isGameOver = false;
        currentPosition = 0;
        UpdateActiveSprite();
    }

    public int GetCurrentPosition()
    {
        return currentPosition;
    }
}