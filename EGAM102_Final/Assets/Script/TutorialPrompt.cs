using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPromptFadeInOut : MonoBehaviour
{
    public KeyCode dismissKey = KeyCode.Space;
    public float appearDelay = 0f;
    public float fadeDuration = 1f;

    private Text uiText;
    private TextMeshProUGUI tmpText;
    private CanvasGroup canvasGroup;
    private bool fadingOut = false;
    private bool fadedIn = false;

    void Start()
    {
        uiText = GetComponent<Text>();
        tmpText = GetComponent<TextMeshProUGUI>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        Invoke(nameof(FadeIn), appearDelay);
    }

    void FadeIn()
    {
        StartCoroutine(Fade(0f, 1f, fadeDuration));
        fadedIn = true;
    }

    void Update()
    {
        if (!fadingOut && fadedIn && Input.GetKeyDown(dismissKey))
        {
            StartCoroutine(Fade(1f, 0f, fadeDuration, true));
        }
    }

    private System.Collections.IEnumerator Fade(float from, float to, float duration, bool disableOnEnd = false)
    {
        float timer = 0f;
        canvasGroup.alpha = from;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            yield return null;
        }

        canvasGroup.alpha = to;

        if (disableOnEnd)
        {
            fadingOut = true;
            gameObject.SetActive(false);
        }
    }
}
