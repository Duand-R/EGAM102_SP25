using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Horizontal force multiplier
    public float minJumpForce = 5f;
    public float maxJumpForce = 10f;
    public float maxChargeTime = 1.0f;

    private float jumpCharge = 0f;
    private bool isCharging = false;
    private bool isGrounded = true;
    private Rigidbody2D rb;
    private float moveX;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");

        // Start charging jump when grounded and space is pressed.
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpCharge = 0f;
        }

        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            jumpCharge += Time.deltaTime;
            if (jumpCharge > maxChargeTime)
                jumpCharge = maxChargeTime;
        }

        // When space is released, perform the jump.
        if (isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            float normalizedCharge = jumpCharge / maxChargeTime;
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, normalizedCharge);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isCharging = false;
            isGrounded = false;
        }

        // Update Animator parameter for the squash animation
        animator.SetBool("IsCharging", isCharging);
    }

    void FixedUpdate()
    {

        rb.AddForce(new Vector2(moveX * speed, 0), ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Simple ground check
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}

