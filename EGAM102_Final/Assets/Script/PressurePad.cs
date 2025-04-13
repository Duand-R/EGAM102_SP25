using UnityEngine;

public class PressurePad : MonoBehaviour
{
    public Animator padAnimator;
    public DoorController connectedDoor;

    private int objectsOnPad = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTriggeringObject(other))
        {
            objectsOnPad++;
            if (objectsOnPad == 1)
            {
                padAnimator.SetBool("isPressed", true);
                connectedDoor?.OpenDoor();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsTriggeringObject(other))
        {
            objectsOnPad--;
            if (objectsOnPad <= 0)
            {
                padAnimator.SetBool("isPressed", false);
                connectedDoor?.CloseDoor();
            }
        }
    }

    private bool IsTriggeringObject(Collider2D col)
    {
        return col.CompareTag("Player") || col.CompareTag("Pushable");
    }
}
