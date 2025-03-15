using UnityEngine;

public class FreeSegmentController : MonoBehaviour
{
    private HingeJoint2D hinge;

    // Motor speeds (in degrees per second)
    // When mouse is held, the free segment rotates inward (closes)
    public float closeMotorSpeed = 200f;
    // When mouse is released, it rotates back (opens)
    public float openMotorSpeed = -200f;

    // Maximum torque the motor can apply
    public float maxMotorTorque = 500f;

    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        if (hinge == null)
        {
            Debug.LogError("HingeJoint2D is missing on " + gameObject.name);
        }

        // Set up joint limits: open position (0°) to closed position (30°)
        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = 0f;
        limits.max = 90f;
        hinge.limits = limits;
        hinge.useLimits = true;
    }

    void Update()
    {
        JointMotor2D motor = hinge.motor;
        // When mouse button is held, drive toward the closed angle (contract)
        if (Input.GetMouseButton(0))
        {
            motor.motorSpeed = closeMotorSpeed;
        }
        else // When not pressed, drive toward the open angle (release)
        {
            motor.motorSpeed = openMotorSpeed;
        }
        motor.maxMotorTorque = maxMotorTorque;
        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
