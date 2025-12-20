// This script handles what happens when the ball hits a peg.

using UnityEngine;

public class PegBehavior : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private ScoreManager scoreManagerReference;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change peg behavior
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private int pointValueForThisPeg = 100;
    [SerializeField] private float secondsToWaitBeforeRemovingPeg = 0.5f;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The peg tracks this while playing
    // ─────────────────────────────────────────────────────────────────────────

    private bool thisPegHasAlreadyBeenHit = false;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (thisPegHasAlreadyBeenHit)
        {
            return;
        }

        // Why check the tag instead of assuming anything that hits is a ball?
        bool objectThatHitUsWasTheBall = collision.gameObject.CompareTag("Ball");

        if (objectThatHitUsWasTheBall)
        {
            HandlePegWasHitByBall();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void HandlePegWasHitByBall()
    {
        thisPegHasAlreadyBeenHit = true;

        scoreManagerReference.AddPointsFromPegHit(pointValueForThisPeg);

        // Why not destroy immediately?
        StartCoroutine(FadeOutThenDestroy());
    }

    private System.Collections.IEnumerator FadeOutThenDestroy()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < secondsToWaitBeforeRemovingPeg)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / secondsToWaitBeforeRemovingPeg;

            // What does Lerp do? Try changing 0f to 0.5f.
            float newAlpha = Mathf.Lerp(originalColor.a, 0f, percentComplete);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
