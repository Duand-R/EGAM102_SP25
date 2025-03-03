using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManholeCover : MonoBehaviour
{
    [Header("Mobile settings")]
    public float moveSpeed = 5.0f;
    public Transform[] positions;
    public int currentPositionIndex = 0;

    [Header("component reference")]
    public Animator animator;
    public VirtualButton leftButton;
    public VirtualButton rightButton;

    private bool isMoving = false;

    private void Start()
    {
        if (positions.Length > 0 && animator != null)
        {
            // starting location
            transform.position = positions[currentPositionIndex].position;
        }
        else
        {
            Debug.LogError("ManholeCover: Missing parts!");
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver())
        {
            return;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        // move left
        if (Input.GetKeyDown(KeyCode.A) && !isMoving && currentPositionIndex > 0)
        {
            isMoving = true;
            currentPositionIndex--;

            StartCoroutine(MoveToPosition(positions[currentPositionIndex].position));

            // triger animation
            if (animator != null)
            {
                animator.SetTrigger("MoveLeft");
            }

            // update visual button state
            if (leftButton != null)
            {
                leftButton.Press();
            }
        }

        // move right
        if (Input.GetKeyDown(KeyCode.D) && !isMoving && currentPositionIndex < positions.Length - 1)
        {
            isMoving = true;
            currentPositionIndex++;

            StartCoroutine(MoveToPosition(positions[currentPositionIndex].position));

            // trigger animation
            if (animator != null)
            {
                animator.SetTrigger("MoveRight");
            }

            // update visual button
            if (rightButton != null)
            {
                rightButton.Press();
            }
        }

        // release button
        if (Input.GetKeyUp(KeyCode.A) && leftButton != null)
        {
            leftButton.Release();
        }

        if (Input.GetKeyUp(KeyCode.D) && rightButton != null)
        {
            rightButton.Release();
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (transform.position != targetPosition)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            if (fractionOfJourney >= 1.0f)
            {
                transform.position = targetPosition;
            }

            yield return null;
        }

        isMoving = false;
    }

    // check Pedestrian safe pass
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Pedestrian"))
        {
            Pedestrian pedestrian = other.GetComponent<Pedestrian>();
            if (pedestrian != null)
            {
                pedestrian.SetSafe(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pedestrian"))
        {
            Pedestrian pedestrian = other.GetComponent<Pedestrian>();
            if (pedestrian != null)
            {
                pedestrian.SetSafe(false);
            }
        }
    }
}