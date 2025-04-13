using UnityEngine;

public class RespawnablePickup : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    private Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Respawn()
    {
        transform.SetParent(null);
        transform.position = startPos;
        transform.rotation = startRot;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

        }
    }
}
