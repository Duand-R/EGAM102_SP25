using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    [Header("MovementSetting")]
    public float walkSpeed = 1.0f;
    public Transform[] waypoints;
    public bool startFromLeft = true;

    [Header("component reference")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private int currentWaypoint = 0;
    private bool isSafe = false;
    private bool isFalling = false;
    private bool hasFinished = false;

    private void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("Pedestrian: No waypoint set!");
            return;
        }

        // Set initial position and direction
        transform.position = waypoints[currentWaypoint].position;

        // Set walking direction
        if (!startFromLeft)
        {
            // If starting from the right, flip the pedestrian
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = true;
            }

            // When starting from the right, reverse the waypoint order
            System.Array.Reverse(waypoints);
        }

        // start walking
        animator.SetBool("IsWalking", true);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver() || isFalling || hasFinished)
        {
            return;
        }

        // moving pedestrians
        MoveToNextWaypoint();

        // Check whether it is in a dangerous location and unsafe
        CheckFall();
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypoint >= waypoints.Length)
        {
            // The pedestrian has completed all waypoints
            FinishPath();
            return;
        }

        // Get next waypoint
        Vector3 targetPosition = waypoints[currentWaypoint].position;

        // Compute moves
        float step = walkSpeed * GameManager.Instance.GetGameSpeed() * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Check if the current waypoint has been reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Reach waypoint, move to next
            currentWaypoint++;
        }
    }

    private void CheckFall()
    {
        // Determine whether it is on a hole and not covered by a manhole cover
        if (IsOverHole() && !isSafe)
        {
            StartFalling();
        }
    }

    private bool IsOverHole()
    {

        return (currentWaypoint > 0 && currentWaypoint < waypoints.Length &&
                currentWaypoint % 2 == 1);
    }

    private void StartFalling()
    {
        isFalling = true;

        // Trigger falling animation
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Fall");
        }

        // Delayed destruction of objects
        GameManager.Instance.LoseLife();
        Destroy(gameObject, 1.5f);
    }

    private void FinishPath()
    {
        hasFinished = true;

        // Pedestrians cross safely
        GameManager.Instance.AddScore(10);

        // destroy object
        Destroy(gameObject, 0.5f);
    }

    public void SetSafe(bool safe)
    {
        isSafe = safe;
    }
}