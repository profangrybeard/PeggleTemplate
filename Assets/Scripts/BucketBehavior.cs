// This script controls the bucket that moves at the bottom of the screen.

using UnityEngine;

public class BucketBehavior : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private GameManager gameManagerReference;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change bucket behavior
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private float bucketMoveSpeedInUnitsPerSecond = 3f;
    [SerializeField] private float leftBoundaryXPosition = -7f;
    [SerializeField] private float rightBoundaryXPosition = 7f;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The bucket tracks this while playing
    // ─────────────────────────────────────────────────────────────────────────

    private bool bucketIsCurrentlyMovingRight = true;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Update()
    {
        MoveBucketBackAndForth();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Why use OnTriggerEnter2D instead of OnCollisionEnter2D?
        bool objectThatEnteredWasTheBall = other.CompareTag("Ball");

        if (objectThatEnteredWasTheBall)
        {
            HandleBallCaughtByBucket(other.gameObject);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void MoveBucketBackAndForth()
    {
        float moveAmountThisFrame = bucketMoveSpeedInUnitsPerSecond * Time.deltaTime;

        // Why multiply by 1 or -1 instead of having two separate move lines?
        float directionMultiplier = bucketIsCurrentlyMovingRight ? 1f : -1f;

        Vector3 currentPosition = transform.position;
        currentPosition.x += moveAmountThisFrame * directionMultiplier;
        transform.position = currentPosition;

        // Why not use an "if/else if" here?
        if (currentPosition.x >= rightBoundaryXPosition)
        {
            bucketIsCurrentlyMovingRight = false;
        }
        if (currentPosition.x <= leftBoundaryXPosition)
        {
            bucketIsCurrentlyMovingRight = true;
        }
    }

    private void HandleBallCaughtByBucket(GameObject ballThatWasCaught)
    {
        // Why destroy the ball here instead of letting it fall through?
        Destroy(ballThatWasCaught);

        gameManagerReference.OnBallWasCaughtByBucket();
    }
}
