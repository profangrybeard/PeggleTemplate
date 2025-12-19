// This script handles what happens when the ball hits a peg.

using UnityEngine;

// What does "enum" do? Try commenting this out and see what error you get.
public enum PegType
{
    Blue,       // Standard peg - just gives points
    Orange,     // Target peg - must clear all orange pegs to win
    Green,      // Special peg - activates a power-up
    Purple      // Bonus peg - extra points
}

public class PegBehavior : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private ScoreManager scoreManagerReference;
    [SerializeField] private GameManager gameManagerReference;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change peg behavior
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private PegType whatTypeOfPegIsThis = PegType.Blue;
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
        // Why check if it's already been hit? What happens if we don't?
        if (thisPegHasAlreadyBeenHit)
        {
            return;
        }

        // Why check the tag instead of just assuming anything that hits is a ball?
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

        // Tell the ScoreManager we got hit
        scoreManagerReference.AddPointsFromPegHit(pointValueForThisPeg);

        // Why tell the GameManager separately if we're an orange peg?
        if (whatTypeOfPegIsThis == PegType.Orange)
        {
            gameManagerReference.OnOrangePegWasHit();
        }

        // Why not destroy immediately? What would we miss?
        StartCoroutine(FadeOutThenDestroy());
    }

    private System.Collections.IEnumerator FadeOutThenDestroy()
    {
        // Why use a coroutine instead of Destroy(gameObject, delay)?
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < secondsToWaitBeforeRemovingPeg)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / secondsToWaitBeforeRemovingPeg;

            // What does Lerp do? Try changing the 0f to 0.5f and see what happens.
            float newAlpha = Mathf.Lerp(originalColor.a, 0f, percentComplete);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
