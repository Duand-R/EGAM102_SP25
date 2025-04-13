using UnityEngine;

public class StickWall : MonoBehaviour
{
    public LayerMask stickWallLayer;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color stickColor = new Color(0f, 1f, 0.5f);

    private bool isSticking = false;
    private bool onVerticalWall = false;

    private float detachCooldown = 0.2f;
    private float detachTimer = 0f;

    private SlimeSFX sfx;

    public bool IsSticking() => isSticking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        rb.freezeRotation = true;
        sfx = GetComponent<SlimeSFX>();
    }

    void Update()
    {
        if (detachTimer > 0f)
            detachTimer -= Time.deltaTime;

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

            // Wall SFX based on movement
            if ((onVerticalWall && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f) ||
                (!onVerticalWall && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f))
            {
                sfx?.PlayWallMove();
            }
            else
            {
                sfx?.StopWallMove();
            }
        }
        else
        {
            sfx?.StopWallMove();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & stickWallLayer) != 0 && detachTimer <= 0f)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                if (normal.y < -0.5f) // Ceiling
                {
                    onVerticalWall = false;
                    StickToWall();
                    break;
                }
                else if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) // Wall
                {
                    onVerticalWall = true;
                    StickToWall();
                    break;
                }
                else if (normal.y > 0.5f) // Ground (optional)
                {
                    onVerticalWall = false;
                    StickToWall();
                    break;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isSticking || detachTimer > 0f) return;

        if (((1 << collision.gameObject.layer) & stickWallLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                if (normal.y < -0.5f)
                {
                    onVerticalWall = false;
                    StickToWall();
                    break;
                }
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
        if (detachTimer > 0f || isSticking) return;

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
        detachTimer = detachCooldown;

        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        spriteRenderer.color = originalColor;

        sfx?.StopWallMove();
    }
}
