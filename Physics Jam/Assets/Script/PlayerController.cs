using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float minJumpForce = 5f;
    public float maxJumpForce = 10f;
    public float maxChargeTime = 1.0f;

    // Trail Renderer for jump trail effect.
    public TrailRenderer jumpTrail;
    // Particle System for landing dust effect (child of the player).
    public ParticleSystem landingDust;

    private float jumpCharge = 0f;
    private bool isCharging = false;
    private bool isGrounded = true;
    private bool wasGrounded = true;  // Tracks previous grounded state.
    private Rigidbody2D rb;
    private float moveX;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (jumpTrail != null)
        {
            jumpTrail.emitting = false;
        }

        if (landingDust != null)
        {
            landingDust.Stop();
        }
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");

        // If on the ground and Space is pressed, start charging jump.
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpCharge = 0f;
            Debug.Log("Started charging jump.");
        }

        // Accumulate jump charge while holding Space.
        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            jumpCharge += Time.deltaTime;
            if (jumpCharge > maxChargeTime)
                jumpCharge = maxChargeTime;
        }

        // When Space is released, perform the jump.
        if (isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            float normalizedCharge = jumpCharge / maxChargeTime;
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, normalizedCharge);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isCharging = false;
            // Mark as airborne.
            isGrounded = false;
            wasGrounded = false;
            Debug.Log("Jump executed with force: " + jumpForce);

            if (jumpTrail != null)
            {
                jumpTrail.emitting = true;
            }
        }

        if (animator != null)
        {
            animator.SetBool("IsCharging", isCharging);
        }

        // If grounded, ensure the trail stops and clears.
        if (isGrounded && jumpTrail != null && jumpTrail.emitting)
        {
            jumpTrail.emitting = false;
            jumpTrail.Clear();
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector2(moveX * speed, 0), ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for contact from below (indicating landing).
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            // Only trigger the dust burst if the player was airborne.
            if (!wasGrounded)
            {
                if (landingDust != null)
                {
                    // Reset the landing dust so the burst plays fresh.
                    landingDust.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    landingDust.transform.localPosition = new Vector3(0, -0.5f, 0);
                    landingDust.Play();
                    Debug.Log("Playing landing dust.");
                }
            }
            // Now mark the player as grounded.
            isGrounded = true;
            wasGrounded = true;
            Debug.Log("Landed.");
        }
    }
}
