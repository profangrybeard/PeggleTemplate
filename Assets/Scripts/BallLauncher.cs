// This script handles aiming and launching the ball.

using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private GameManager gameManagerReference;
    [SerializeField] private GameObject ballPrefabToSpawn;
    [SerializeField] private Transform positionWhereBallSpawns;
    [SerializeField] private Camera mainCameraReference;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change launcher behavior
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private float launchForceMultiplier = 10f;
    [SerializeField] private float maximumAimAngleInDegrees = 75f;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The launcher tracks these while playing
    // ─────────────────────────────────────────────────────────────────────────

    private float currentAimAngleInDegrees = 0f;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Update()
    {
        if (!gameManagerReference.PlayerIsAllowedToShoot())
        {
            return;
        }

        AimTowardMousePosition();
        CheckForLaunchInput();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void AimTowardMousePosition()
    {
        // Convert mouse position from screen pixels to world units.
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 10f;
        Vector3 mouseWorldPosition = mainCameraReference.ScreenToWorldPoint(mouseScreenPosition);

        // Calculate direction from launcher to mouse.
        Vector3 directionToMouse = mouseWorldPosition - transform.position;

        // Convert direction to angle, adjusting so 0° means straight down.
        float angleInDegrees = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg + 90f;

        // Clamp so player can only aim downward into the play field.
        currentAimAngleInDegrees = Mathf.Clamp(angleInDegrees, -maximumAimAngleInDegrees, maximumAimAngleInDegrees);

        // Apply rotation.
        transform.rotation = Quaternion.Euler(0f, 0f, currentAimAngleInDegrees);
    }

    private void CheckForLaunchInput()
    {
        // Why use GetMouseButtonDown instead of GetMouseButton?
        bool playerClickedThisFrame = Input.GetMouseButtonDown(0);
        bool playerPressedSpaceThisFrame = Input.GetKeyDown(KeyCode.Space);

        if (playerClickedThisFrame || playerPressedSpaceThisFrame)
        {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        GameObject newBall = Instantiate(ballPrefabToSpawn, positionWhereBallSpawns.position, Quaternion.identity);

        // Why negative? The launcher points down, but transform.up points up.
        Vector2 launchDirection = -transform.up;
        Vector2 launchVelocity = launchDirection * launchForceMultiplier;

        Rigidbody2D ballRigidbody = newBall.GetComponent<Rigidbody2D>();
        ballRigidbody.linearVelocity = launchVelocity;

        gameManagerReference.OnBallWasLaunched();
    }
}
