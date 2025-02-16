using UnityEngine;

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

    private EnemyState currentState;
    private GameObject target;
    private float pauseTimer;
    void Start()
    {
        currentState = EnemyState.Searching;
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
}
