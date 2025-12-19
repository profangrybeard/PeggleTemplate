// This script handles aiming and launching the ball.

using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private GameManager gameManagerReference;
    [SerializeField] private GameObject ballPrefabToSpawn;
    [SerializeField] private Transform positionWhereBalSpawns;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change launcher behavior
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private float launchForceMultiplier = 10f;
    [SerializeField] private float maximumAimAngleInDegrees = 75f;
    [SerializeField] private float aimingSpeedInDegreesPerSecond = 90f;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The launcher tracks these while playing
    // ─────────────────────────────────────────────────────────────────────────

    private float currentAimAngleInDegrees = 0f;
    private bool launcherIsCurrentlyAimingRight = true;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Update()
    {
        // Why check with GameManager before allowing input?
        if (!gameManagerReference.PlayerIsAllowedToShoot())
        {
            return;
        }

        UpdateAimAngle();
        RotateLauncherToMatchAimAngle();
        CheckForLaunchInput();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void UpdateAimAngle()
    {
        // Why auto-aim instead of using mouse position?
        float aimChangeThisFrame = aimingSpeedInDegreesPerSecond * Time.deltaTime;

        if (launcherIsCurrentlyAimingRight)
        {
            currentAimAngleInDegrees -= aimChangeThisFrame;
        }
        else
        {
            currentAimAngleInDegrees += aimChangeThisFrame;
        }

        // Why flip direction at the boundaries instead of clamping?
        if (currentAimAngleInDegrees <= -maximumAimAngleInDegrees)
        {
            launcherIsCurrentlyAimingRight = false;
        }
        else if (currentAimAngleInDegrees >= maximumAimAngleInDegrees)
        {
            launcherIsCurrentlyAimingRight = true;
        }
    }

    private void RotateLauncherToMatchAimAngle()
    {
        // What does Quaternion.Euler do? Try removing this line.
        transform.rotation = Quaternion.Euler(0f, 0f, currentAimAngleInDegrees);
    }

    private void CheckForLaunchInput()
    {
        // Why use GetKeyDown instead of GetKey?
        bool playerPressedSpaceThisFrame = Input.GetKeyDown(KeyCode.Space);
        bool playerClickedMouseThisFrame = Input.GetMouseButtonDown(0);

        if (playerPressedSpaceThisFrame || playerClickedMouseThisFrame)
        {
            LaunchBallInCurrentDirection();
        }
    }

    private void LaunchBallInCurrentDirection()
    {
        // Why spawn at a separate position instead of the launcher's position?
        GameObject newBall = Instantiate(ballPrefabToSpawn, positionWhereBalSpawns.position, Quaternion.identity);

        // Why does transform.up give us the launch direction?
        Vector2 launchDirection = transform.up;
        Vector2 launchVelocity = launchDirection * launchForceMultiplier;

        // Why use GetComponent here instead of storing a reference?
        Rigidbody2D ballRigidbody = newBall.GetComponent<Rigidbody2D>();
        ballRigidbody.linearVelocity = launchVelocity;

        gameManagerReference.OnBallWasLaunched();
    }
}
