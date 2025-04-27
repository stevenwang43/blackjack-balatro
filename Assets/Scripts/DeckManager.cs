using System.Collections.Generic;
using UnityEngine;
using Blackjack;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public List<Card> deckCards = new List<Card>();

    void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("Standard Deck");
        allCards.AddRange(cards);
        ResetDeck();
    }

    public void DrawCard(HandManager handManager)
    {
        handManager.AddCardToHand(deckCards[0]);
        deckCards.RemoveAt(0);
    }

    public void ResetDeck()
    {
        deckCards.Clear();
        deckCards.AddRange(allCards);
        Shuffle();
    }

    private void Shuffle()
    {
        for (int i = deckCards.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }
}
