using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Slider staminaSlider;
    public SwimmerController swimmerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        staminaSlider.value = swimmerController.currentStamina;
    }
}
