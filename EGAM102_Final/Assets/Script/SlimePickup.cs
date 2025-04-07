using UnityEngine;

public class SlimePickup : MonoBehaviour
{
    public Transform carryPoint; // Assign in Inspector
    public KeyCode interactKey = KeyCode.E;

    private GameObject carriedObject = null;
    private bool canCarry = false;
    private Collider2D nearbyObject = null;

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
    }

    void Drop()
    {
        carriedObject.transform.SetParent(null);

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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pushable"))
        {
            canCarry = false;
            nearbyObject = null;
        }
    }
}
