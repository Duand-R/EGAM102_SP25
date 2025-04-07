using UnityEngine;

public class StartUIController : MonoBehaviour
{
    public GameObject startCanvas; // Assign in Inspector
    public GameObject slimeController; // The slime GameObject with control script

    private void Start()
    {
        slimeController.SetActive(false); // Disable movement at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            startCanvas.SetActive(false); // Hide UI
            slimeController.SetActive(true); // Enable gameplay
        }
    }
}
