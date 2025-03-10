using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // Assign your player transform in the Inspector
    public Vector3 offset;      // Set an offset as needed

    void LateUpdate()
    {
        // Follow the player's position
        transform.position = player.position + offset;
        // Lock the rotation
        transform.rotation = Quaternion.identity;
    }
}
