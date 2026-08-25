// This script handles what happens when the ball hits a peg.

using UnityEngine;

// What does "enum" do? Try commenting this out and see what error you get.
public enum PegType
{
    Blue,       // Scores points. Nothing else.
    Orange,     // Scores points AND counts toward winning.
    Green,      // Scores points. Where would a power-up go?
    Purple      // Scores points. What would make this one a bonus?
}

public class PegBehavior : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private ScoreManager scoreManagerReference;

    // Why does this peg need to know about TWO different managers?
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

    private void Start()
    {
        ApplyColorThatMatchesPegType();
    }

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

    private void ApplyColorThatMatchesPegType()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Why a switch here instead of four separate if-statements?
        switch (whatTypeOfPegIsThis)
        {
            case PegType.Blue:
                spriteRenderer.color = new Color(0.25f, 0.55f, 1f);
                break;
            case PegType.Orange:
                spriteRenderer.color = new Color(1f, 0.5f, 0.1f);
                break;
            case PegType.Green:
                spriteRenderer.color = new Color(0.3f, 0.85f, 0.3f);
                break;
            case PegType.Purple:
                spriteRenderer.color = new Color(0.7f, 0.35f, 0.9f);
                break;
        }
    }

    private void HandlePegWasHitByBall()
    {
        thisPegHasAlreadyBeenHit = true;

        // Every peg scores. Why is this line outside the if-check below?
        scoreManagerReference.AddPointsFromPegHit(pointValueForThisPeg);

        // Only orange pegs report to the GameManager. Why only orange?
        if (whatTypeOfPegIsThis == PegType.Orange)
        {
            gameManagerReference.OnOrangePegWasHit();
        }

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
