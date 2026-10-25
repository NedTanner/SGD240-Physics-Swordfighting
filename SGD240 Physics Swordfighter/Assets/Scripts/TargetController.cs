using UnityEngine;

public enum GuardType
{
    High,
    Mid,
    Low
}

public class TargetController : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private TargetJoint2D targetJoint;
    [SerializeField] private GameObject targetPointer;

    [Header("Guard Type")]
    [SerializeField] private GuardType currentGuard;
    private Vector2 highGuardPos = new Vector2(7f, 2.5f); // Not very customisable, but I already customised them and my word is FINAL
    private Vector2 midGuardPos = new Vector2(7f, 1.5f);
    private Vector2 lowGuardPos = new Vector2(6f, 1f);

    [Header("Player")]
    [SerializeField] private int playerNumber;

    private void Awake()
    {
        if (playerNumber == 2) // Inverts local pointer position for Player 2
        {
            highGuardPos = new Vector2(-highGuardPos.x, 2.5f);
            midGuardPos = new Vector2(-midGuardPos.x, 1.5f);
            lowGuardPos = new Vector2(-lowGuardPos.x, 1f);
        }
    }

    private void FixedUpdate()
    {
        switch (currentGuard)
        {
            case GuardType.High:
                targetPointer.transform.localPosition = new Vector2(highGuardPos.x, highGuardPos.y);
                break;
            case GuardType.Mid:
                targetPointer.transform.localPosition = new Vector2(midGuardPos.x, midGuardPos.y);
                break;
            case GuardType.Low:
                targetPointer.transform.localPosition = new Vector2(lowGuardPos.x, lowGuardPos.y);
                break;
        }

        targetJoint.target = (Vector2)targetPointer.transform.position;
    }

    public void ChangeGuard(int guardIndex) // For UI interactions
    {
        if (guardIndex == 0)
        {
            currentGuard = GuardType.Low;
        }

        else if (guardIndex == 1)
        {
            currentGuard = GuardType.Mid;
        }

        else if (guardIndex == 2)
        {
            currentGuard = GuardType.High;
        }
    }

}
