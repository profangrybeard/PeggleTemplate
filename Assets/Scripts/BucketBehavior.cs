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
    [SerializeField] private float secondsTheBucketPopLasts = 0.25f;
    [SerializeField] private float howMuchBiggerTheBucketGetsWhenItCatches = 1.25f;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The bucket tracks these while playing
    // ─────────────────────────────────────────────────────────────────────────

    private bool bucketIsCurrentlyMovingRight = true;
    private Vector3 bucketSizeWhenNotPopping;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Start()
    {
        // Why remember the size now instead of reading it when a ball is caught?
        bucketSizeWhenNotPopping = transform.localScale;
    }

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

        // The trigger collider is on this same object, so it grows during the pop.
        // For a quarter of a second the bucket is physically wider. Why is that safe?
        StartCoroutine(PopTheBucketThenReturnToNormalSize());

        gameManagerReference.OnBallWasCaughtByBucket();
    }

    private System.Collections.IEnumerator PopTheBucketThenReturnToNormalSize()
    {
        Vector3 poppedSize = bucketSizeWhenNotPopping * howMuchBiggerTheBucketGetsWhenItCatches;

        float elapsedTime = 0f;

        while (elapsedTime < secondsTheBucketPopLasts)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / secondsTheBucketPopLasts;

            // PingPong counts 0 → 1 → 0. Why is that better here than Lerp?
            float howPoppedRightNow = Mathf.PingPong(percentComplete * 2f, 1f);

            transform.localScale = Vector3.Lerp(bucketSizeWhenNotPopping, poppedSize, howPoppedRightNow);

            yield return null;
        }

        // Why set the size explicitly instead of trusting the loop to land back on it?
        transform.localScale = bucketSizeWhenNotPopping;
    }
}
