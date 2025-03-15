using UnityEngine;

public class NailAnchor : MonoBehaviour
{

    public string playerTag = "Player";


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(playerTag))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Add a FixedJoint2D to the player
                FixedJoint2D joint = collision.gameObject.GetComponent<FixedJoint2D>();
                if (joint == null)
                {
                    joint = collision.gameObject.AddComponent<FixedJoint2D>();
                }
                // Connect the joint to this nail's Rigidbody2D.
                joint.connectedBody = GetComponent<Rigidbody2D>();


                Debug.Log("Player anchored to nail!");
            }
        }
    }
}

