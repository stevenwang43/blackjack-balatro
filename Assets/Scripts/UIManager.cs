using UnityEngine;
using Blackjack;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameplayPanelController gameplayPanelController;
    public StartPanelController startPanelController;
    public ShopPanelController shopPanelController;

    public HandManager playerHand;
    public Dealer dealer;
    public GameManager gameManager;
    public MainManager mainManager;

    private Card[] loadedCards; // All available cards loaded from Resources

    void Start()
    {
        LoadCards();

        // Setup the Start Panel
        startPanelController.SetupStartButton(() => mainManager.StartGame());
    }

    private void LoadCards()
    {
        loadedCards = Resources.LoadAll<Card>("Cards");
        if (loadedCards == null || loadedCards.Length == 0)
        {
            Debug.LogError("No cards found in Resources/Cards!");
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Started");

        gameplayPanelController.SetupButtons(
            onHit: () => gameManager.PlayerDraw(),
            onStand: () => gameManager.PlayerStand()
        );

        UpdateGameplayUI();
    }

    public void OpenShop()
    {
        Debug.Log("Opening Shop");

        Card[] shopCards = PickRandomCards(3);

        shopPanelController.SetupShop(
            title: "Card Shop",
            playerMoney: mainManager.PlayerMoney,
            availableCards: shopCards,
            onBuy: (index) => OnBuyCard(shopCards[index])
        );

        // TODO: Hide gameplay panel, show shop panel (if you're toggling panels manually)
        gameplayPanelController.gameObject.SetActive(false);
        shopPanelController.gameObject.SetActive(true);
    }

    private void OnBuyCard(Card card)
    {
        Debug.Log("Bought card: " + card.displayValue);

        if (mainManager.PlayerMoney >= GetCardPrice(card))
        {
            mainManager.PlayerMoney -= GetCardPrice(card);
            playerHand.AddCardToHand(card); // Example: add directly to hand
            UpdateGameplayUI();
            OpenShop(); // Refresh shop if you want, or close shop manually
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    private int GetCardPrice(Card card)
    {
        if (card.cardType.Contains(Card.CardType.Joker)) return 50;
        if (card.cardType.Contains(Card.CardType.Consumable)) return 20;
        return 10;
    }

    private Card[] PickRandomCards(int count)
    {
        List<Card> shuffled = new List<Card>(loadedCards);

        for (int i = 0; i < shuffled.Count; i++)
        {
            int j = Random.Range(0, shuffled.Count);
            var temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count)).ToArray();
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
