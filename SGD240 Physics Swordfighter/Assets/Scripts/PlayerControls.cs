using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private InputAction raiseBicep;
    private InputAction raiseForearm;

    [SerializeField] private HingeJoint2D bicepJoint;
    [SerializeField] private HingeJoint2D forearmJoint;

    private void Awake()
    {
        raiseBicep = InputSystem.actions.FindAction("P1 Raise Bicep");
        raiseForearm = InputSystem.actions.FindAction("P1 Raise Forearm");

        raiseBicep.performed += _ => RaiseBicep();
        raiseForearm.performed += _ => RaiseForearm();

        raiseBicep.canceled += _ => ReleaseBicep();
        raiseForearm.canceled += _ => ReleaseForearm();
    }

    private void RaiseBicep()
    {
        bicepJoint.useMotor = true;
    }

    private void RaiseForearm()
    {
        forearmJoint.useMotor = true;
    }

    private void ReleaseBicep()
    {
        bicepJoint.useMotor = false;
    }

    private void ReleaseForearm()
    {
        forearmJoint.useMotor = false;
    }
}
