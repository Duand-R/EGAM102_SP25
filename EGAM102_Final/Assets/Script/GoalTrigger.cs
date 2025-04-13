using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public float pulseDuration = 1.2f;  // Total time to animate
    public float pulseSpeed = 5f;       // How fast it pulses
    public float scaleAmount = 1.5f;    // Max size

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PulseThenLoadNextScene());
        }
    }

    private System.Collections.IEnumerator PulseThenLoadNextScene()
    {
        float timer = 0f;
        Vector3 originalScale = transform.localScale;

        while (timer < pulseDuration)
        {
            float scale = 1 + Mathf.Sin(timer * pulseSpeed) * (scaleAmount - 1);
            transform.localScale = originalScale * scale;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        gameObject.SetActive(false); // Hide goal object

        // Load the next scene
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            Debug.Log("No more scenes! Game complete!");
    }
}
