using UnityEngine;

public class BallFallOnClick : MonoBehaviour
{
    private Rigidbody2D rb;

    public float activeGravityScale = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Disable gravity initially.
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // When the player clicks the mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Enable gravity on the ball.
            rb.gravityScale = activeGravityScale;
        }
    }
}
