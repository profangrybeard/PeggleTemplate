// This script controls what happens to the ball after it's launched.

using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private GameManager gameManagerReference;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change how the ball behaves
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private float minimumYPositionBeforeBallIsLost = -6f;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The ball tracks these while playing
    // ─────────────────────────────────────────────────────────────────────────

    private bool ballHasAlreadyNotifiedGameManagerItWasLost = false;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Update()
    {
        CheckIfBallFellOffScreen();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void CheckIfBallFellOffScreen()
    {
        // Why check position instead of using a collider trigger at the bottom?
        bool ballIsNowBelowTheScreen = transform.position.y < minimumYPositionBeforeBallIsLost;

        if (ballIsNowBelowTheScreen && !ballHasAlreadyNotifiedGameManagerItWasLost)
        {
            // Why use a flag instead of just destroying immediately?
            ballHasAlreadyNotifiedGameManagerItWasLost = true;

            gameManagerReference.OnBallWasLost();

            // Why wait a moment before destroying instead of destroying right away?
            Destroy(gameObject, 0.1f);
        }
    }
}
