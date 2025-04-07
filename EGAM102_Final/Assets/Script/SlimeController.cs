using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer; // Set this in the Inspector

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Lock rotation
    }

    void Update()
    {
        // Move left/right
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Jump if grounded
        if (Input.GetKeyDown(KeyCode.Space) && IsGroundedBox())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Better grounded check using BoxCast
    bool IsGroundedBox()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(
            GetComponent<Collider2D>().bounds.center,
            GetComponent<Collider2D>().bounds.size,
            0f,
            Vector2.down,
            extraHeight,
            groundLayer
        );

        //Debug.Log("Box Grounded: " + (hit.collider != null));
        return hit.collider != null;
    }

    // Optional: draw the BoxCast in Scene view
    void OnDrawGizmos()
    {
        if (GetComponent<Collider2D>() == null) return;

        Gizmos.color = IsGroundedBox() ? Color.green : Color.red;
        Gizmos.DrawWireCube(
            GetComponent<Collider2D>().bounds.center + Vector3.down * 0.1f,
            GetComponent<Collider2D>().bounds.size
        );
    }
}
