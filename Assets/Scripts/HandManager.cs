using UnityEngine;
using Blackjack;
using System.Collections.Generic;
using System;
using System.Linq;

public class HandManager : MonoBehaviour
{

    public DeckManager deckManager;
    public GameObject card;
    public Transform handTransform;
    public float cardSpacing = 2f;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public List<Card> cardsData = new List<Card>();

    public void ResetHand()
    {
        foreach (GameObject card in cardsInHand)
        {
            Destroy(card);
        }
        cardsInHand.Clear();
        cardsData.Clear();
    }

    public int GetTotal()
    {
        float total = 0;
        int aces = 0;
        float blackjackThreshold = 21f;

        if (ModifierManager.Instance != null)
        {
            blackjackThreshold = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.BlackjackThreshold);
        }

        foreach (Card c in cardsData)
        {
            // Handle Aces separately as they can be either 1 or 11
            if (c.displayValue == "A")
            {
                aces++;
                total += 1; // Add 1 for Ace initially
            }
            // Face cards (J, Q, K) all count as 10
            else if (c.displayValue == "J" || c.displayValue == "Q" || c.displayValue == "K")
            {
                total += 10; // Face cards are worth 10
            }
            // For numbered cards, use the displayValue to determine their value
            else if (int.TryParse(c.displayValue, out int cardValue))
            {
                total += cardValue; // Convert numeric displayValue to an integer and add
            }
        }

        // Adjust Ace(s) to be 11 if it doesn't cause the total to exceed 21
        for (int i = 0; i < aces; i++)
        {
            if (total + 10 <= blackjackThreshold)
            {
                total += 10;
            }
        }

        return Mathf.RoundToInt(total);;
    }

    public int GetValueTotal()
    {
        // Get modifiers if they exist
        float cardValueMultiplier = 1.0f;
        float blackjackThreshold = 21f;

        if (ModifierManager.Instance != null)
        {
            cardValueMultiplier = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.CardValueMultiplier);
            blackjackThreshold = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.BlackjackThreshold);
        }

        // Step 1: Reset card values based on their displayValue
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Card c = cardsInHand[i].GetComponent<CardDisplay>().cardData; // Get the actual Card data from the card's display component

            // Reset the card's value to its display value (A = 11, JQK = 10, and numeric cards retain their value)
            if (c.displayValue == "A")
            {
                c.value = 11; // Ace is 11
            }
            else if (c.displayValue == "J" || c.displayValue == "Q" || c.displayValue == "K")
            {
                c.value = 10; // Jack, Queen, King are 10
            }
            else
            {
                c.value = int.Parse(c.displayValue); // Numeric cards use their displayValue as the value
            }
        }

        // Step 2: Apply multiplier for adjacent cards
        for (int i = 0; i < cardsInHand.Count - 1; i++)  // Ensure we don't go out of range
        {
            Card c = cardsInHand[i].GetComponent<CardDisplay>().cardData;  // Get the current card's data
            Card nextCard = cardsInHand[i + 1].GetComponent<CardDisplay>().cardData;  // Get the next card's data

            if (c.displayValue == nextCard.displayValue)
            {
                // If adjacent cards have the same displayValue, multiply the value by 2 for both cards
                c.value *= 2; // Apply multiplier to the current card
                nextCard.value *= 2;  // Apply multiplier to the next card
                i++;
            }
        }

        // Step 3: Calculate the total value
        float total = 0;
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Card c = cardsInHand[i].GetComponent<CardDisplay>().cardData; // Get the actual Card data from the card's display component
            float cardValue = c.value * cardValueMultiplier;  // Default card value with multiplier applied
            total += cardValue;  // Add the current card's value to the total
        }

        // Apply hand total bonus if ModifierManager exists
        if (ModifierManager.Instance != null)
        {
            float bonus = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.HandTotalBonus);
            total += bonus;
        }

        if (GetTotal() == blackjackThreshold) {
            total *= 2;
        } else if (GetTotal() >= blackjackThreshold) {
            total = 0;
        }

        return Mathf.RoundToInt(total);
    }
    

    public void AddCardToHand(Card cardData)
    {
        GameObject newCard = Instantiate(card, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);
        cardsData.Add(cardData);
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }

    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        for (int i = 0; i < cardCount; i++) {
            float horizontalOffset = i * cardSpacing;
            Vector3 newPosition = new Vector3(horizontalOffset, 0f, 0f);
            cardsInHand[i].GetComponent<DragUIObject>().SetPosition(newPosition);
            cardsInHand[i].GetComponentInChildren<Canvas>().sortingOrder = -i;
        }
    }

    public void ReorderCards()
    {
        // Sort the cards based on their x position to ensure they're arranged correctly
        cardsInHand.Sort((a, b) => a.transform.localPosition.x.CompareTo(b.transform.localPosition.x));

        // Set the sorting order to stack them from left to right
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            float horizontalOffset = i * cardSpacing;
            Vector3 newPosition = new Vector3(horizontalOffset, 0f, 0f);

            // Set the card's position
            cardsInHand[i].GetComponent<DragUIObject>().SetPosition(newPosition);
        }
    }


}
