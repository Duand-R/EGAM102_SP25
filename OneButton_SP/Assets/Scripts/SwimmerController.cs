using UnityEngine;
using System;

public class SwimmerController : MonoBehaviour
{
    public float swimForce = 5f;
    public float maxStamina = 100f;
    public float staminaDepletionRate = 1f;
    public float staminaRegenRate = 1f;
    public float currentStamina;

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
            GameOver("You ran out of stamina and drowned!");
        }
    }
    void GameOver(string message)
    {
        Debug.Log(message);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
