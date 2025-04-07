using UnityEngine;

public class PressurePad : MonoBehaviour
{
    public GameObject door;
    public Vector3 pressedScale = new Vector3(1f, 0.5f, 1f);
    public float squishSpeed = 10f;

    private DoorController doorScript;
    private Vector3 originalScale;
    private int objectCount = 0;
    private bool isPressed = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        originalScale = transform.localScale;

        if (door != null)
            doorScript = door.GetComponent<DoorController>();
    }

    private void Update()
    {
        Vector3 targetScale = isPressed ? pressedScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * squishSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            objectCount++;
            if (objectCount == 1)
            {
                animator.SetBool("IsPressed", true);
                if (doorScript != null)
                    doorScript.OpenDoor(); // only open ONCE
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            objectCount--;
            if (objectCount <= 0)
            {
                objectCount = 0; // safety reset
                animator.SetBool("IsPressed", false);
                if (doorScript != null)
                    doorScript.CloseDoor(); // only close ONCE
            }
        }
    }
}

