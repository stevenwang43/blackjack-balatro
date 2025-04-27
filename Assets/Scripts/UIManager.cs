using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameplayPanelController gameplayPanelController;
    public StartPanelController startPanelController;

    public HandManager playerHand;
    public Dealer dealer;
    public GameManager gameManager;
    public MainManager mainManager;
    public DeckManager playerDeck;

    [Header("Modifier UI")]
    public ModifierUIManager modifierUIManager;

    void Start()
    {
        // Setup the Start Panel
        startPanelController.SetupStartButton(() => mainManager.StartGame());
    }

    public void StartGame()
    {
        Debug.Log("Game Started");
        gameplayPanelController.SetupButtons(
            onHit: () => gameManager.PlayerDraw(),
            onStand: () => gameManager.PlayerStand()
        );
        UpdateGameplayUI();

        // Make sure the modifier UI is set up
        if (modifierUIManager == null)
        {
            modifierUIManager = GetComponent<ModifierUIManager>();
            if (modifierUIManager == null)
            {
                modifierUIManager = gameObject.AddComponent<ModifierUIManager>();
            }
        }
    }

    public void UpdateGameplayUI()
    {
        gameplayPanelController.UpdateHandTotals(
            playerHand.GetTotal(),
            playerHand.GetValueTotal(),
            dealer.HandTotal()
        );

        gameplayPanelController.UpdateScore(gameManager.Score);
        gameplayPanelController.UpdateGameCount(gameManager.playerRoundsWon, gameManager.playerRoundsLost);
        gameplayPanelController.UpdateDeckText(playerDeck.deckCards.Count, playerDeck.allCards.Count);
        gameplayPanelController.UpdateDealerHealthText((int)dealer.health, (int)dealer.maxHealth);

        switch (gameManager.state)
        {
            case GameManager.GameState.PlayerTurn:
                gameplayPanelController.SetGameStateUI("", true);
                break;
            case GameManager.GameState.DealerTurn:
                gameplayPanelController.SetGameStateUI("Dealer's Turn", false);
                break;
            case GameManager.GameState.RoundWon:
                gameplayPanelController.SetGameStateUI("You dealt " + Mathf.Max(dealer.damageTaken, 0) + " damage!", false);
                break;
            case GameManager.GameState.RoundLost:
                gameplayPanelController.SetGameStateUI("Busted!", false);
                break;
        }
    }
}
