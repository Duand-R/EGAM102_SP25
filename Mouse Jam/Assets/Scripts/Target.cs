using UnityEngine;

public class Target : MonoBehaviour
{
    public int points = 1;
    public float lifeTime = 2f;

    private GameManager gameManager;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        Destroy(gameObject, lifeTime);
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.IncreaseScore();
        }
        Destroy(gameObject);
    }
}
