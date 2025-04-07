using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameObject endCanvas; // Drag your EndCanvas here
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
            StartCoroutine(PulseThenDisappear());

            if (endCanvas != null)
            {
                endCanvas.SetActive(true);
                Time.timeScale = 0f; // Optional: pause the game
            }
        }
    }

    private System.Collections.IEnumerator PulseThenDisappear()
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

        gameObject.SetActive(false); // Disappear!
    }
}
