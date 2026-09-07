using UnityEditor.Compilation;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HingeJoint2D bicepJoint;
    [SerializeField] private HingeJoint2D forearmJoint;
    private InputAction raiseBicep;
    private InputAction raiseForearm;
    private InputAction stepIn;
    private InputAction stepBack;

    [Header("Movement")]
    [SerializeField] private float minStepDistance;
    [SerializeField] private float maxStepDistance;
    private float stepDistance;

    [Header("Player")]
    public int playerNumber;

    private void Awake()
    {
        raiseBicep = InputSystem.actions.FindAction("P" + playerNumber + " Bicep"); // Modularity, allows reuse of this code between players
        raiseForearm = InputSystem.actions.FindAction("P" + playerNumber + " Forearm");

        stepIn = InputSystem.actions.FindAction("P" + playerNumber + " Step In");
        stepBack = InputSystem.actions.FindAction("P" + playerNumber + "Step Back");

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

    private void CalculateStep()
    {

    }


}
