using UnityEngine;

public class SharkController : MonoBehaviour
{
    public float speed = 2f;
    public float retreatDistance = 1f;
    private Transform player;
    private SwimmerController swimmerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        swimmerController = player.GetComponent<SwimmerController>();

        swimmerController.OnSwim += OnPlayerSwim;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void OnPlayerSwim()
    {
        Vector2 retreatDirection = (transform.position - player.position).normalized;
        transform.position += (Vector3)retreatDirection * retreatDistance;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameOver("You were eaten by the shark!");
        }
    }

    void GameOver(string message)
    {
        Debug.Log(message);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    void OnDestroy()
    {

        if (swimmerController != null)
        {
            swimmerController.OnSwim -= OnPlayerSwim;
        }
    }
}
