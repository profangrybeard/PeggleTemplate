// This script controls the overall flow of the game.

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private UIController uiControllerReference;
    [SerializeField] private ScoreManager scoreManagerReference;
    [SerializeField] private BallLauncher ballLauncherReference;

    // ─────────────────────────────────────────────────────────────────────────
    // SETTINGS - Tweak these to change game rules
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private int numberOfBallsPlayerStartsWith = 10;
    [SerializeField] private int totalOrangePegsInThisLevel = 25;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The game manager tracks these while playing
    // ─────────────────────────────────────────────────────────────────────────

    private int numberOfBallsRemaining = 0;
    private int orangePegsStillRemaining = 0;
    private bool gameIsCurrentlyActive = false;
    private bool aBallIsCurrentlyInPlay = false;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY MESSAGES - Unity calls these automatically
    // ─────────────────────────────────────────────────────────────────────────

    private void Start()
    {
        StartNewGame();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────

    public bool PlayerIsAllowedToShoot()
    {
        // Why check multiple conditions instead of just one?
        bool gameIsStillGoing = gameIsCurrentlyActive;
        bool noBallIsCurrentlyActive = !aBallIsCurrentlyInPlay;
        bool playerHasBallsLeft = numberOfBallsRemaining > 0;

        return gameIsStillGoing && noBallIsCurrentlyActive && playerHasBallsLeft;
    }

    public void OnBallWasLaunched()
    {
        aBallIsCurrentlyInPlay = true;
        numberOfBallsRemaining--;

        uiControllerReference.UpdateBallsRemainingDisplay(numberOfBallsRemaining);
    }

    public void OnBallWasLost()
    {
        aBallIsCurrentlyInPlay = false;

        // Why reset the combo here instead of when the ball is launched?
        scoreManagerReference.ResetComboForNewShot();

        // Why check win/lose AFTER the ball is gone, not during play?
        CheckForWinOrLoseCondition();
    }

    public void OnBallWasCaughtByBucket()
    {
        aBallIsCurrentlyInPlay = false;

        // Why give the ball back here instead of just adding points?
        numberOfBallsRemaining++;

        uiControllerReference.UpdateBallsRemainingDisplay(numberOfBallsRemaining);

        scoreManagerReference.ResetComboForNewShot();
    }

    public void OnOrangePegWasHit()
    {
        orangePegsStillRemaining--;

        // Why not check for win here? The player just hit an orange peg!
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE METHODS - Internal logic
    // ─────────────────────────────────────────────────────────────────────────

    private void StartNewGame()
    {
        numberOfBallsRemaining = numberOfBallsPlayerStartsWith;
        orangePegsStillRemaining = totalOrangePegsInThisLevel;
        gameIsCurrentlyActive = true;
        aBallIsCurrentlyInPlay = false;

        scoreManagerReference.ResetScoreForNewGame();
        uiControllerReference.UpdateBallsRemainingDisplay(numberOfBallsRemaining);
        uiControllerReference.HideGameOverMessage();
    }

    private void CheckForWinOrLoseCondition()
    {
        // Why check for win first?
        bool playerHasClearedAllOrangePegs = orangePegsStillRemaining <= 0;

        if (playerHasClearedAllOrangePegs)
        {
            EndGameWithResult(true);
            return;
        }

        bool playerHasNoBallsLeft = numberOfBallsRemaining <= 0;

        if (playerHasNoBallsLeft)
        {
            EndGameWithResult(false);
        }

        // What happens if neither condition is true?
    }

    private void EndGameWithResult(bool playerWon)
    {
        gameIsCurrentlyActive = false;

        uiControllerReference.ShowGameOverMessage(playerWon);
    }
}
