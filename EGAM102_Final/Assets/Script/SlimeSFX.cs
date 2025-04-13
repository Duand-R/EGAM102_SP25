using UnityEngine;

public class SlimeSFX : MonoBehaviour
{
    [Header("One-shot Sounds")]
    public AudioClip jumpSFX;
    public AudioClip pickupSFX;
    public AudioClip detachSFX;
    public AudioClip wallMoveSFX;

    private AudioSource source;
    private bool isWallMoving = false;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        if (jumpSFX != null)
            source.PlayOneShot(jumpSFX);
    }

    public void PlayPickup()
    {
        if (pickupSFX != null)
            source.PlayOneShot(pickupSFX);
    }

    public void PlayDetach()
    {
        if (detachSFX != null)
            source.PlayOneShot(detachSFX);
    }

    public void PlayWallMove()
    {
        if (wallMoveSFX != null && !isWallMoving)
        {
            source.PlayOneShot(wallMoveSFX);
            isWallMoving = true;
        }
    }

    public void StopWallMove()
    {
        isWallMoving = false;
    }
}
