using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameplayPanelController gameplayPanelController;
    public HandManager playerHand;
    public Dealer dealer;
    public GameManager gameManager;

    void Start()
    {
        gameplayPanelController.SetupButtons(
            onHit: () => gameManager.PlayerDraw(),
            onStand: () => gameManager.PlayerStand()
        );
        UpdateGameplayUI();
    }

    public void UpdateGameplayUI()
    {
        gameplayPanelController.UpdateHandTotals(
            playerHand.GetTotal(),
            dealer.HandTotal()
        );

        gameplayPanelController.UpdateScore(gameManager.Score);
        gameplayPanelController.UpdateGameCount(gameManager.playerRoundsWon, gameManager.playerRoundsLost);

        switch (gameManager.state)
        {
            case GameManager.GameState.PlayerTurn:
                gameplayPanelController.SetGameStateUI("", true);
                break;
            case GameManager.GameState.DealerTurn:
                gameplayPanelController.SetGameStateUI("Dealer's Turn", false);
                break;
            case GameManager.GameState.RoundWon:
                gameplayPanelController.SetGameStateUI("You Win!", false);
                break;
            case GameManager.GameState.RoundLost:
                gameplayPanelController.SetGameStateUI("You Lose!", false);
                break;
        }
    }
}
