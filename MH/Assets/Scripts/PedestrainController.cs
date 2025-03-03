using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    public enum MovementDirection { LeftToRight, RightToLeft }
    public enum PedestrianState { Walking, Falling }

    public MovementDirection moveDirection;
    public bool isUpperLevel; 

    public float walkSpeed = 1.0f;
    public Transform startPosition;
    public Transform endPosition;

    private Animator animator;
    private PedestrianState currentState = PedestrianState.Walking;
    private GameManager gameManager;
    private ManholeController manholeController;
    private bool crossedHole = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        manholeController = FindAnyObjectByType<ManholeController>();

        transform.position = startPosition.position;

        
        if (moveDirection == MovementDirection.RightToLeft)
        {
            
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Update()
    {
        if (gameManager.IsGameOver()) return;

        if (currentState == PedestrianState.Walking)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, walkSpeed * Time.deltaTime);

            
            CheckManholeCollision();

            
            if (Vector3.Distance(transform.position, endPosition.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    void CheckManholeCollision()
    {
        
        int currentManholePos = manholeController.GetCurrentPosition();

        
        float posX = transform.position.x;

        
        float hole1Min = -2.0f;
        float hole1Max = -1.0f;
        float hole2Min = 1.0f;
        float hole2Max = 2.0f;

        
        if (!crossedHole)
        {
            
            if (isUpperLevel && posX > hole1Min && posX < hole1Max)
            {
                
                if (currentManholePos != 0)
                {
                    Fall();
                }
                else
                {
                    crossedHole = true;
                    gameManager.AddScore();
                }
            }
            
            else if (isUpperLevel && posX > hole2Min && posX < hole2Max)
            {
                
                if (currentManholePos != 1)
                {
                    Fall();
                }
                else
                {
                    crossedHole = true;
                    gameManager.AddScore();
                }
            }
            
            else if (!isUpperLevel && posX > hole1Min && posX < hole1Max)
            {
                
                if (currentManholePos != 2)
                {
                    Fall();
                }
                else
                {
                    crossedHole = true;
                    gameManager.AddScore();
                }
            }
            
            else if (!isUpperLevel && posX > hole2Min && posX < hole2Max)
            {
                
                if (currentManholePos != 3)
                {
                    Fall();
                }
                else
                {
                    crossedHole = true;
                    gameManager.AddScore();
                }
            }
        }
    }

    void Fall()
    {
        currentState = PedestrianState.Falling;
        animator.SetTrigger("Fall");

        
        gameManager.LoseLife(transform.position);

        
        Destroy(gameObject, 1.0f);
    }
}