using UnityEngine;
using System.Collections;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public float squashDelay = 0.15f;

    private Rigidbody2D rb;
    private Animator animator;
    private StickWall stickWall;
    private bool jumpInProgress = false;
    private float currentVelocityX;
    private SlimeSFX sfx;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stickWall = GetComponent<StickWall>();
        rb.freezeRotation = true;
        sfx = GetComponent<SlimeSFX>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float targetVelocityX = moveInput * moveSpeed;
        currentVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX, Time.deltaTime * 10f);
        rb.linearVelocity = new Vector2(currentVelocityX, rb.linearVelocity.y);

        bool grounded = IsGroundedBox();
        bool sticking = stickWall != null && stickWall.IsSticking();

        animator.SetBool("isGrounded", grounded);
        animator.SetBool("isSticking", sticking);

        // Smooth squash/stretch while moving
        if (Mathf.Abs(currentVelocityX) > 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.1f, 0.9f, 1), Time.deltaTime * 10f);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 10f);
        }

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !jumpInProgress)
        {
            StartCoroutine(SquashThenJump());
        }
        if (Input.GetKeyDown(KeyCode.Space))
            sfx.PlayJump();

        if (Input.GetKeyDown(KeyCode.E))
            sfx.PlayPickup();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            sfx.PlayDetach();
    }

    private IEnumerator SquashThenJump()
    {
        jumpInProgress = true;

        animator.SetTrigger("jumpPressed");

        yield return new WaitForSeconds(squashDelay); // delay after squash anim

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity += new Vector2(0f, jumpForce);

        jumpInProgress = false;
    }

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

        return hit.collider != null;
    }

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

