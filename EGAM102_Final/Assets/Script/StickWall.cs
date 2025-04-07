using UnityEngine;

public class StickWall : MonoBehaviour
{
    public LayerMask stickWallLayer;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color stickColor = new Color(0f, 1f, 0.5f); // neon green

    private bool isSticking = false;
    private bool onVerticalWall = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

    }

    void Update()
    {
        if (isSticking)
        {
            float input = onVerticalWall ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Horizontal");

            Vector2 velocity = onVerticalWall
                ? new Vector2(0f, input * moveSpeed)
                : new Vector2(input * moveSpeed, 0f);

            rb.linearVelocity = velocity;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DetachFromWall();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & stickWallLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                onVerticalWall = Mathf.Abs(normal.x) > Mathf.Abs(normal.y);
                StickToWall();
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & stickWallLayer) != 0)
        {
            DetachFromWall();
        }
    }

    void StickToWall()
    {
        isSticking = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        rb.constraints = onVerticalWall
            ? RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation
            : RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        spriteRenderer.color = stickColor;
    }

    void DetachFromWall()
    {
        isSticking = false;
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        spriteRenderer.color = originalColor;
    }
}
