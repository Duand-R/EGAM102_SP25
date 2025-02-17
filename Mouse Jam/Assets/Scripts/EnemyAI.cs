using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Searching,
        Moving,
        Attacking,
        Paused 
    }

    public float moveSpeed = 3f;
    public float attackRange = 1f;
    public float pauseDuration = 2f; 
    public int maxHealth = 3; 
    public Image[] healthHearts;
    public AudioClip eatSFX;

    private EnemyState currentState;
    private GameObject target;
    private float pauseTimer;
    private GameManager gameManager;
    private int currentHealth;
    private AudioSource audioSource;

    void Start()
    {
        currentState = EnemyState.Searching;
        gameManager = FindAnyObjectByType<GameManager>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Searching:
                SearchForTarget();
                break;
            case EnemyState.Moving:
                MoveToTarget();
                break;
            case EnemyState.Attacking:
                AttackTarget();
                break;
            case EnemyState.Paused:
                Pause();
                break;
        }
    }

    void SearchForTarget()
    {
        target = GameObject.FindWithTag("Target");
        if (target != null)
        {
            currentState = EnemyState.Moving;
        }
    }

    void MoveToTarget()
    {
        if (target == null)
        {
            currentState = EnemyState.Searching;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
    }

    void AttackTarget()
    {
        if (target == null)
        {
            currentState = EnemyState.Searching;
            return;
        }

        Destroy(target);
        if (eatSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(eatSFX);
        }

        TakeDamage(1);
        currentState = EnemyState.Searching;
    }

    void Pause()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            currentState = EnemyState.Searching;
        }
    }

    void OnMouseDown()
    {
        if (currentState != EnemyState.Paused)
        {
            currentState = EnemyState.Paused;
            pauseTimer = pauseDuration;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Baby is out of health!");
            gameManager.GameOver();
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthHearts.Length; i++)
        {
            healthHearts[i].enabled = i < currentHealth;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}