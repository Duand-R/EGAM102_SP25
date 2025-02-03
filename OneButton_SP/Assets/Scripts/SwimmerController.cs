using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SwimmerController : MonoBehaviour
{
    public float swimForce = 5f;
    public float maxStamina = 100f;
    public float staminaDepletionRate = 1f;
    public float staminaRegenRate = 1f;
    public float currentStamina;
    public Collider2D playerCollider;

    public float CurrentStamina
    {
        get { return currentStamina; }
    }
    public event Action OnSwim;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentStamina > 0)
        {
            Swim();
        }

        RegenerateStamina();
        CheckStamina();
        Debug.Log(currentStamina);
    }

    void Swim()
    {
        rb.linearVelocity = Vector2.up * swimForce;
        currentStamina -= staminaDepletionRate;

        OnSwim?.Invoke();
    }
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }
    }

    void CheckStamina()
    {
        if (currentStamina <= 0)
        {
            SceneManager.LoadScene("DrownScene");
            
        }
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shark"))
        {
            SceneManager.LoadScene("LoseScene");
            Destroy(collision.gameObject);
        }
    }
}
