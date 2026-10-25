using UnityEngine;

public class CamAdjuster : MonoBehaviour
{
    [Header("Camera Parameters")]
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private Camera mainCamera;

    private float currentZoom;

    private float currentDistance;
    private float minDistance;
    private float maxDistance;

    private Vector3 cameraVelocity = Vector3.zero;

    [Header("Players")]
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;

    private float playerOneX => playerOne.transform.position.x;
    private float playerTwoX => playerTwo.transform.position.x;

    private void Awake()
    {
        minDistance = Mathf.Abs(playerOneX - playerTwoX); // Dynamic distance detection + creation
        maxDistance = minDistance * 3f;
    }

    private void LateUpdate()
    {
        currentDistance = Mathf.Abs(playerOneX - playerTwoX);
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance); // Constrain distance between bounds

        currentZoom = (currentDistance - minDistance) / maxDistance;

        Camera.main.orthographicSize = Mathf.Lerp(maxZoom, minZoom, currentZoom); // zoomLevel determined by distance between min and max distance

        float middlePoint = Mathf.Lerp(playerOneX, playerTwoX, 0.5f);
        Vector3 cameraTarget = new Vector3(middlePoint, 0f, -20f);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, cameraTarget, ref cameraVelocity, 0.1f); // Smoothly tracks camera position to target, reduces jitter
    }

}
