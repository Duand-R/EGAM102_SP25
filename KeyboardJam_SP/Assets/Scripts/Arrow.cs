using UnityEngine;

public class Arrow : MonoBehaviour
{
    public string direction;
    public float lifetime;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on the arrow!");
        }
    }
    public void Initialize(string dir, float life)
    {
        direction = dir;
        lifetime = life;

        switch (direction)
        {
            case "Up":
                spriteRenderer.sprite = upSprite;
                break;
            case "Down":
                spriteRenderer.sprite = downSprite;
                break;
            case "Left":
                spriteRenderer.sprite = leftSprite;
                break;
            case "Right":
                spriteRenderer.sprite = rightSprite;
                break;
            default:
                Debug.LogError("Unknown direction: " + direction);
                break;
        }

        Destroy(gameObject, lifetime);
    }
}
