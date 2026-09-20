using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlayerBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HingeJoint2D bicepJoint;
    [SerializeField] private HingeJoint2D forearmJoint;
    private InputAction raiseBicep;
    private InputAction raiseForearm;
    private InputAction moveAction;

    [Header("Movement")]
    [SerializeField] private float minStepDistance;
    [SerializeField] private float defaultMaxStep;
    [SerializeField] private float lungeCooldown;
    [SerializeField] private bool isCharging;
    private float maxStepDistance;
    private float stepDistance;
    private Vector2 directionModifier;
    private float lungeCharge;
    private float lungeCooldownTimer;
    private Vector2 lungeDestination;

    [Header("Player")]
    public int playerNumber;
    private int oppositePlayer = 1;

    private void Awake()
    {
        raiseBicep = InputSystem.actions.FindAction("P" + playerNumber + " Bicep"); // Modularity, allows reuse of this code between players
        raiseForearm = InputSystem.actions.FindAction("P" + playerNumber + " Forearm");

        moveAction = InputSystem.actions.FindAction("P" + playerNumber + " Movement");

        raiseBicep.performed += _ => RaiseBicep();
        raiseForearm.performed += _ => RaiseForearm();

        raiseBicep.canceled += _ => ReleaseBicep();
        raiseForearm.canceled += _ => ReleaseForearm();

        moveAction.performed += _ => LungeCheck();
        moveAction.canceled += _ => Lunge();

        oppositePlayer = (playerNumber == 1) ? 2 : 1; // Checks if player is 1 or 2, sets opposite number to... you guessed it the opposite!
        lungeDestination = transform.position;
    }

    private void Update()
    {
        directionModifier = moveAction.ReadValue<Vector2>(); // Only really need the x value, either -1,0,1 and multiplies relevant x values based on this modifier

        if (lungeCooldownTimer > 0)
        {
            lungeCooldownTimer -= Time.deltaTime;
        }

        if (isCharging)
        {
            Vector2 raycastStart = new Vector2(transform.position.x + (minStepDistance * directionModifier.x), transform.position.y);

            RaycastHit2D hit = Physics2D.Raycast(raycastStart, directionModifier, defaultMaxStep);

            if (hit)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player " + oppositePlayer)) // Moves destination away from obstacle, inverts based on direction. Detects opposite player
                {
                    lungeDestination = (Vector2)hit.transform.position;
                    lungeDestination.x -= 2f * directionModifier.x;
                    lungeDestination.x = (lungeDestination.normalized.x < minStepDistance * directionModifier.x) ? lungeDestination.x = minStepDistance * directionModifier.x : lungeDestination.x;
                    //  If player's (normalised) lunge distance is less than their minimum step distance, it sets it to their minstep distance multiplied by which direction they're facing instead
                }

                else
                {
                    lungeDestination = raycastStart + (defaultMaxStep * directionModifier);

                    Debug.DrawRay(raycastStart, minStepDistance * directionModifier, Color.pink);
                }

            }

            else
            {
                lungeDestination = (Vector2)transform.position + (defaultMaxStep * directionModifier);

                Debug.DrawRay(raycastStart, minStepDistance * directionModifier, Color.pink);
            }
        }

        // Draw Forward axis (Z) in Blue
        Debug.DrawRay(lungeDestination, transform.forward * 3f, Color.blue);

        // Draw Up axis (Y) in Green
        Debug.DrawRay(lungeDestination, transform.up * 3f, Color.green);

        // Draw Right axis (X) in Red
        Debug.DrawRay(lungeDestination, transform.right * 3f, Color.red);
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

    private void LungeCheck() // The leftovers of a nightmarish coding journey, I don't know if I even need this anymore. I'm keeping it.
    {
        if (lungeCooldownTimer <= 0)
        {
            lungeCooldownTimer = Time.deltaTime + lungeCooldown;
            isCharging = true;
        }

        else
        {
            return;
        }
    }

    private void Lunge()
    {

    }


}
