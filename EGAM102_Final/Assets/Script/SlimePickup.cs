using UnityEngine;

public class SlimePickup : MonoBehaviour
{
    public Transform carryPoint;
    public KeyCode interactKey = KeyCode.E;
    public GameObject eKeyPrompt;

    private GameObject carriedObject = null;
    private bool canCarry = false;
    private Collider2D nearbyObject = null;

    private SpriteRenderer slimeSprite;
    public Color normalColor = Color.white;
    public Color pickupColor = Color.yellow;

    private bool hasPickedUpOnce = false;

    void Start()
    {
        slimeSprite = GetComponent<SpriteRenderer>();
        if (slimeSprite != null)
        {
            slimeSprite.color = normalColor;
        }

        if (eKeyPrompt != null)
        {
            eKeyPrompt.SetActive(false); // hide at start
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (carriedObject == null && canCarry && nearbyObject != null)
            {
                PickUp(nearbyObject.gameObject);
            }
            else if (carriedObject != null)
            {
                Drop();
            }
        }
    }

    void PickUp(GameObject obj)
    {
        carriedObject = obj;
        carriedObject.transform.position = carryPoint.position;
        carriedObject.transform.SetParent(carryPoint.transform);

        Rigidbody2D rb = carriedObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        if (eKeyPrompt != null)
        {
            eKeyPrompt.SetActive(false); // hide prompt forever
        }

        hasPickedUpOnce = true; // one-time flag
    }

    void Drop()
    {
        Vector2 dropDirection = Vector2.right;
        float input = Input.GetAxisRaw("Horizontal");
        if (input < 0) dropDirection = Vector2.left;
        else if (input > 0) dropDirection = Vector2.right;

        Vector2 dropPosition = transform.position + (Vector3)dropDirection * 1f;

        carriedObject.transform.SetParent(null);
        carriedObject.transform.position = dropPosition;

        Rigidbody2D rb = carriedObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        carriedObject = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pushable"))
        {
            canCarry = true;
            nearbyObject = other;

            if (slimeSprite != null && !IsWallSticking())
                slimeSprite.color = pickupColor;


            if (eKeyPrompt != null && !hasPickedUpOnce)
                eKeyPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pushable"))
        {
            canCarry = false;
            nearbyObject = null;

            if (slimeSprite != null && !IsWallSticking())
                slimeSprite.color = normalColor;


            if (eKeyPrompt != null)
                eKeyPrompt.SetActive(false);
        }
    }
    bool IsWallSticking()
    {
        StickWall stickWall = GetComponent<StickWall>();
        if (stickWall != null)
            return stickWall.IsSticking();
        return false;
    }

}
