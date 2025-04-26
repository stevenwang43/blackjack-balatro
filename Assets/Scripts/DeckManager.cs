using System.Collections.Generic;
using UnityEngine;
using Blackjack;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private int currentIndex = 0;

    void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");
        allCards.AddRange(cards);
        Shuffle(); // Shuffle the deck after loading
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0) {
            return;
        }

        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }

    public void ResetDeck()
    {
        currentIndex = 0;
        Shuffle();
    }

    private void Shuffle()
    {
        for (int i = allCards.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = allCards[i];
            allCards[i] = allCards[j];
            allCards[j] = temp;
        }

        currentIndex = 0; // Reset the draw index after shuffle
    }
}
