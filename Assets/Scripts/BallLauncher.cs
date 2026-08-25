// This script handles aiming and launching the ball.

using UnityEngine;
using UnityEngine.InputSystem;

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
    // INPUT ACTIONS - What the player can DO, and what counts as doing it
    // ─────────────────────────────────────────────────────────────────────────
    //
    // An InputAction is a NAME for something the player can do ("Launch"),
    // separated from the things that trigger it (a mouse button, the spacebar,
    // a gamepad trigger). Those triggers are called BINDINGS.
    //
    // Click the arrow next to these in the Inspector to see their bindings.
    // Notice that the code below never mentions a mouse or a keyboard.

    [SerializeField] private InputAction aimPositionAction;
    [SerializeField] private InputAction launchAction;

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

    private void OnEnable()
    {
        // An action is asleep until you wake it up. Nothing is read until then.
        // Why does this belong in OnEnable() instead of Start()?
        aimPositionAction.Enable();
        launchAction.Enable();
    }

    private void OnDisable()
    {
        // Why turn them off again? What is still listening if we don't?
        aimPositionAction.Disable();
        launchAction.Disable();
    }

    private void Update()
    {
        if (!gameManagerReference.PlayerIsAllowedToShoot())
        {
            return;
        }

        AimTowardPointer();
        CheckForLaunchInput();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void AimTowardPointer()
    {
        // Ask the action for its value right now. A Value action always has one.
        // Why does this return a Vector2 when ScreenToWorldPoint wants a Vector3?
        Vector2 pointerScreenPosition = aimPositionAction.ReadValue<Vector2>();

        // Screen pixels are not world units. This converts between them.
        Vector3 pointerScreenPositionWithDepth =
            new Vector3(pointerScreenPosition.x, pointerScreenPosition.y, 10f);
        Vector3 pointerWorldPosition =
            mainCameraReference.ScreenToWorldPoint(pointerScreenPositionWithDepth);

        // Calculate direction from launcher to pointer.
        Vector3 directionToPointer = pointerWorldPosition - transform.position;

        // Convert direction to angle, adjusting so 0° means straight down.
        float angleInDegrees = Mathf.Atan2(directionToPointer.y, directionToPointer.x) * Mathf.Rad2Deg + 90f;

        // Clamp so player can only aim downward into the play field.
        currentAimAngleInDegrees = Mathf.Clamp(angleInDegrees, -maximumAimAngleInDegrees, maximumAimAngleInDegrees);

        // Apply rotation.
        transform.rotation = Quaternion.Euler(0f, 0f, currentAimAngleInDegrees);
    }

    private void CheckForLaunchInput()
    {
        // "Was it pressed during this frame?" - true for exactly one frame.
        // The launch action has TWO bindings. Why does one line handle both?
        bool playerTriggeredLaunchThisFrame = launchAction.WasPressedThisFrame();

        if (playerTriggeredLaunchThisFrame)
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
