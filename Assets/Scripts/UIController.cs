// This script updates all the UI elements on screen.

using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private TextMeshProUGUI scoreDisplayText;
    [SerializeField] private TextMeshProUGUI ballsRemainingDisplayText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverMessageText;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change how earning a ball back feels
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private float secondsTheBallCountCelebrationLasts = 0.4f;
    [SerializeField] private float howMuchBiggerTheBallCountGets = 1.5f;
    [SerializeField] private Color colorTheBallCountFlashes = Color.green;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - Remembered so the celebration knows what to put back
    // ─────────────────────────────────────────────────────────────────────────

    private Vector3 ballCountSizeWhenNotCelebrating;
    private Color ballCountColorWhenNotCelebrating;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Start()
    {
        // Why capture these once at startup instead of when the celebration begins?
        ballCountSizeWhenNotCelebrating = ballsRemainingDisplayText.transform.localScale;
        ballCountColorWhenNotCelebrating = ballsRemainingDisplayText.color;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────

    public void UpdateScoreDisplay(int newScore)
    {
        // Why not calculate the score here? This is the UI after all.
        scoreDisplayText.text = "Score: " + newScore.ToString();
    }

    public void UpdateBallsRemainingDisplay(int ballsLeft)
    {
        ballsRemainingDisplayText.text = "Balls: " + ballsLeft.ToString();
    }

    // This runs on top of UpdateBallsRemainingDisplay, not instead of it.
    // Why does earning a ball back need its own method at all?
    public void CelebrateBallEarnedBack()
    {
        StartCoroutine(PopAndFlashTheBallCount());
    }

    public void ShowGameOverMessage(bool playerWon)
    {
        gameOverPanel.SetActive(true);

        // Why use a ternary operator here? What's another way to write this?
        string messageToShow = playerWon ? "You Win!" : "Game Over";
        gameOverMessageText.text = messageToShow;
    }

    public void HideGameOverMessage()
    {
        gameOverPanel.SetActive(false);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private System.Collections.IEnumerator PopAndFlashTheBallCount()
    {
        Vector3 celebratingSize = ballCountSizeWhenNotCelebrating * howMuchBiggerTheBallCountGets;

        float elapsedTime = 0f;

        while (elapsedTime < secondsTheBallCountCelebrationLasts)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / secondsTheBallCountCelebrationLasts;

            // PingPong counts 0 → 1 → 0, so the text swells and settles in one pass.
            float howCelebratoryRightNow = Mathf.PingPong(percentComplete * 2f, 1f);

            ballsRemainingDisplayText.transform.localScale =
                Vector3.Lerp(ballCountSizeWhenNotCelebrating, celebratingSize, howCelebratoryRightNow);

            ballsRemainingDisplayText.color =
                Color.Lerp(ballCountColorWhenNotCelebrating, colorTheBallCountFlashes, howCelebratoryRightNow);

            yield return null;
        }

        // Why snap these back instead of trusting the loop to land on them exactly?
        ballsRemainingDisplayText.transform.localScale = ballCountSizeWhenNotCelebrating;
        ballsRemainingDisplayText.color = ballCountColorWhenNotCelebrating;
    }
}
