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
}
