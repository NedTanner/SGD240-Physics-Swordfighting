using TMPro;
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
    [SerializeField] private float defaultMaxStepDistance;
    [SerializeField] private float lungeCooldown;
    [SerializeField] private float movementSpeed;
    private Vector2 lungeDestination;
    private Vector2 directionModifier;
    private float lungeCooldownTimer;
    private float maxStepDistance;
    private bool isMoving;

    [Header("Lunge Charge Parameters")]
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float chargeSpeed = 1.5f;
    private bool isCharging;
    private float lungeCharge;

    [Header("Player")]
    public int playerNumber;
    private int oppositePlayer = 1;

    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;

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
        maxStepDistance = defaultMaxStepDistance * directionModifier.x;

        playerAnimator.SetInteger("PlayerNumber", playerNumber);
    }

    private void Update()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.Playing)
        {
            directionModifier = moveAction.ReadValue<Vector2>(); // Either -1,0,1 and multiplies relevant x values based on this modifier for left anf right movement

            LayerMask playerLayerMask = LayerMask.GetMask("Player " + oppositePlayer);


            if (lungeCooldownTimer > 0)
            {
                lungeCooldownTimer -= Time.deltaTime;
            }

            if (isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, lungeDestination, movementSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, lungeDestination) < 0.01f) // Keep position consistent + end check
                {
                    transform.position = lungeDestination;
                    isMoving = false;
                }
            }

            if (isCharging)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionModifier, defaultMaxStepDistance + 3f, playerLayerMask);

                maxStepDistance = defaultMaxStepDistance * directionModifier.x;

                if (hit.collider != null)
                {
                    maxStepDistance = hit.transform.position.x - transform.position.x;

                    if (Mathf.Abs(maxStepDistance) <= 5) //  If player's (absoulute) lunge distance is less than boundary float, invert direction
                    {
                        directionModifier.x *= -1f;

                        maxStepDistance = 0;
                    }
                }

                float maxCharge = Mathf.Abs(maxStepDistance);

                lungeCharge += Time.deltaTime * chargeSpeed * directionModifier.x;

                if (directionModifier.x < 0)
                {
                    lungeCharge = Mathf.Clamp(lungeCharge, -maxCharge, -minStepDistance); // Swaps around values to account for larger negative variables, this sucks!
                }

                else
                {
                    lungeCharge = Mathf.Clamp(lungeCharge, minStepDistance, maxCharge);
                }
            }
        }
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
            isCharging = true;
            playerAnimator.SetTrigger("ChargeLunge");
        }

        else
        {
            return;
        }
    }

    private void Lunge()
    {
        if (GameManager.CurrentGameState == GameManager.GameState.Playing)
        {
            if (isCharging)
            {
                lungeDestination = transform.TransformPoint(new Vector2(lungeCharge, 0)); // Marks lunge destination in the world so it stays consistent even if parent transform moves

                playerAnimator.SetFloat("LungeForce", Mathf.Abs(lungeCharge)); // For lunge animation blendtree
                lungeCooldownTimer = lungeCooldown;
                isCharging = false;
                isMoving = true;
                lungeCharge = 0;

                if (playerNumber == 1)
                {
                    if (directionModifier.x < 0)
                    {
                        playerAnimator.SetTrigger("StepBackward");
                    }

                    else
                    {
                        playerAnimator.SetTrigger("StepForward");
                    }
                }

                else if (playerNumber == 2) // Swaps animations to correctly correspond to direction of movement. A drawback of the otherwise modular movement input system
                {
                    if (directionModifier.x < 0)
                    {
                        playerAnimator.SetTrigger("StepForward");
                    }

                    else
                    {
                        playerAnimator.SetTrigger("StepBackward");
                    }
                }

            }

            else
            {
                return;
            }
        }
    }


}
