using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class Blackjack : MonoBehaviour
{
    public TMP_Text playerText, dealerText, resultText;
    public Button hitButton, standButton, restartButton;

    private List<int> playerHand = new List<int>();
    private List<int> dealerHand = new List<int>();
    private bool gameOver = false;

    void Start()
    {
        StartGame();
        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);
        restartButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        playerHand.Clear();
        dealerHand.Clear();
        gameOver = false;
        resultText.text = "";

        playerHand.Add(DrawCard());

        dealerHand.Add(DrawCard());
        dealerHand.Add(DrawCard());

        UpdateUI();
        hitButton.interactable = true;
        standButton.interactable = true;
    }

    int DrawCard()
    {
        int card = Random.Range(1, 14);
        return (card > 10) ? 10 : card; // J, Q, K count as 10
    }

    void PlayerHit()
    {
        if (gameOver) return;
        playerHand.Add(DrawCard());
        UpdateUI();

        if (GetTotal(playerHand) > 21)
        {
            EndGame("You Bust! Dealer Wins.");
        }
    }
        void PlayerStand()
    {
        if (gameOver) return;

        while (GetTotal(dealerHand) < 17)
        {
            dealerHand.Add(DrawCard());
            UpdateUI();
            Thread.Sleep(300);
        }

        int playerTotal = GetTotal(playerHand);
        int dealerTotal = GetTotal(dealerHand);

        if (dealerTotal > 21 || playerTotal > dealerTotal)
            EndGame("You Win!");
        else if (playerTotal == dealerTotal)
            EndGame("Push (Draw).");
        else
            EndGame("Dealer Wins.");
    }

    void EndGame(string result)
    {
        gameOver = true;
        resultText.text = result;
        hitButton.interactable = false;
        standButton.interactable = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        playerText.text = "Player: " + string.Join(", ", playerHand) + " (Total: " + GetTotal(playerHand) + ")";
        Debug.Log(playerText.text);
        dealerText.text = "Dealer: " + string.Join(", ", dealerHand) + (gameOver ? " (Total: " + GetTotal(dealerHand) + ")" : "");
        Debug.Log(dealerText.text);
    }

    int GetTotal(List<int> hand)
    {
        int total = 0, aceCount = 0;
        foreach (int card in hand)
        {
            total += card;
            if (card == 1) aceCount++;
        }

        while (aceCount > 0 && total + 10 <= 21)
        {
            total += 10;
            aceCount--;
        }

        return total;
    }
}