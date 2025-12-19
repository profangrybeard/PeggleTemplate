// This script keeps track of the player's score.

using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // REFERENCES - Drag these in the Inspector
    // ─────────────────────────────────────────────────────────────────────────

    [SerializeField] private UIController uiControllerReference;

    // ─────────────────────────────────────────────────────────────────────────
    // STATE - The score manager tracks these while playing
    // ─────────────────────────────────────────────────────────────────────────

    private int currentPlayerScore = 0;
    private int numberOfPegsHitDuringThisShot = 0;

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC METHODS - Other scripts call these
    // ─────────────────────────────────────────────────────────────────────────

    public void AddPointsFromPegHit(int basePointValueOfPeg)
    {
        numberOfPegsHitDuringThisShot++;

        // Why multiply by the number of pegs hit? What effect does this create?
        int comboMultiplier = numberOfPegsHitDuringThisShot;
        int actualPointsToAdd = basePointValueOfPeg * comboMultiplier;

        currentPlayerScore += actualPointsToAdd;

        uiControllerReference.UpdateScoreDisplay(currentPlayerScore);
    }

    public void ResetComboForNewShot()
    {
        // Why reset only the combo, not the total score?
        numberOfPegsHitDuringThisShot = 0;
    }

    public void ResetScoreForNewGame()
    {
        currentPlayerScore = 0;
        numberOfPegsHitDuringThisShot = 0;

        uiControllerReference.UpdateScoreDisplay(currentPlayerScore);
    }

    public int GetCurrentScore()
    {
        // Why have a getter method instead of making currentPlayerScore public?
        return currentPlayerScore;
    }
}
