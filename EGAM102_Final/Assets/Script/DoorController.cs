using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 5f, 0f);
    public float moveSpeed = 5f;
    public float fadeSpeed = 3f;
    public float fadeInDistance = 0.5f; // When to start fade-in near closed position

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private bool isMoving = false;

    private SpriteRenderer sr;
    private float targetAlpha = 1f;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + offset;

        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // Start visible
    }

    void Update()
    {
        // Move door
        if (isMoving)
        {
            Vector3 targetPos = isOpen ? openPos : closedPos;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, closedPos);

            // Only fade in if returning and near closed position
            if (!isOpen && distance < fadeInDistance)
            {
                targetAlpha = 1f; // Start fade in
            }

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                isMoving = false;
                if (!isOpen)
                {
                    targetAlpha = 1f; // Ensure full visible when fully closed
                }
            }
        }

        // Handle fading
        Color c = sr.color;
        float newAlpha = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
        sr.color = new Color(c.r, c.g, c.b, newAlpha);
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            isMoving = true;
            targetAlpha = 0f; // Fade out immediately
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            isMoving = true;
            targetAlpha = 0f; // Stay transparent until near closed
        }
    }
}
