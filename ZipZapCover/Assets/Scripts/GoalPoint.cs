using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPoint : MonoBehaviour
{
    public List<Rigidbody2D> platformRigidbodies;

    // Time delays for the goal sequence
    public float fallDelay = 2f;       
    public float sceneLoadDelay = 3f;    

    // Animation parameters for the goal point sprite (circle)
    public float expandDuration = 0.5f;  
    public float holdDuration = 1f;      
    public float contractDuration = 0.5f; 

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            Debug.Log("GoalPoint: Player entered trigger.");

            StartCoroutine(GoalSequence());
            StartCoroutine(AnimateGoalPoint());
        }
    }

    private IEnumerator GoalSequence()
    {
        // Wait for fallDelay seconds before making the platforms fall.
        yield return new WaitForSeconds(fallDelay);

        if (platformRigidbodies != null && platformRigidbodies.Count > 0)
        {
            foreach (Rigidbody2D rb in platformRigidbodies)
            {
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Debug.Log("GoalPoint: Platform " + rb.name + " set to Dynamic. It will now fall.");
                }
                else
                {
                    Debug.LogWarning("GoalPoint: One of the platform Rigidbody2D references is missing.");
                }
            }
        }
        else
        {
            Debug.LogWarning("GoalPoint: No platform Rigidbody2D components have been assigned.");
        }

        // Wait the remaining time before loading the scene.
        float remainingDelay = sceneLoadDelay - fallDelay;
        if (remainingDelay > 0)
        {
            yield return new WaitForSeconds(remainingDelay);
        }

        // Determine the next scene based on build index and load it.
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        Debug.Log("GoalPoint: Current scene index = " + currentIndex + ", Next scene index = " + nextIndex);
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("GoalPoint: Loading scene with index " + nextIndex);
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("GoalPoint: No next scene in Build Settings.");
        }
    }

    private IEnumerator AnimateGoalPoint()
    {

        Vector3 originalScale = transform.localScale;
        Vector3 maxScale = originalScale * 1.5f;

        // EXPAND
        float timer = 0f;
        while (timer < expandDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, maxScale, timer / expandDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = maxScale;

        // HOLD
        yield return new WaitForSeconds(holdDuration);

        // CONTRACT
        timer = 0f;
        while (timer < contractDuration)
        {
            transform.localScale = Vector3.Lerp(maxScale, Vector3.zero, timer / contractDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }
}
