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


    void Start()
    {
        
    }

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
        // Get modifiers if they exist
        float cardValueMultiplier = 1.0f;
        float blackjackThreshold = 21f;
        
        if (ModifierManager.Instance != null)
        {
            cardValueMultiplier = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.CardValueMultiplier);
            blackjackThreshold = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.BlackjackThreshold);
        }
        
        // Start with a list containing just 0 as the base score
        List<float> possibleScores = new List<float> { 0 };
        
        // Process each card to generate all possible scores
        foreach (Card c in cardsData)
        {
            int newPossibleScoresCount = possibleScores.Count;
            
            // If this is an ace, it has two possible values (1 or 11)
            if (c.displayValue == "A")
            {
                // For each existing possible score, add two new possibilities: +1 and +11
                for (int i = 0; i < newPossibleScoresCount; i++)
                {
                    float currentScore = possibleScores[i];
                    
                    // Add ace as value 1
                    possibleScores[i] = currentScore + (1 * cardValueMultiplier);
                    
                    // Add a new possibility with ace as value 11
                    possibleScores.Add(currentScore + (11 * cardValueMultiplier));
                }
            }
            else
            {
                // For regular cards, just add their value to all possible scores
                for (int i = 0; i < newPossibleScoresCount; i++)
                {
                    possibleScores[i] += c.value * cardValueMultiplier;
                }
            }
        }
        
        // Apply hand total bonus to all possible scores if ModifierManager exists
        if (ModifierManager.Instance != null)
        {
            float bonus = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.HandTotalBonus);
            for (int i = 0; i < possibleScores.Count; i++)
            {
                possibleScores[i] += bonus;
            }
        }
        
        // Find the best score (highest score that's not over blackjackThreshold)
        float bestScore = 0;
        foreach (float score in possibleScores)
        {
            if (score <= blackjackThreshold && score > bestScore)
            {
                bestScore = score;
            }
        }
        
        // If we don't have any valid scores under the threshold, take the lowest score
        if (bestScore == 0 && possibleScores.Count > 0)
        {
            bestScore = possibleScores.Min();
        }
        
        return Mathf.RoundToInt(bestScore);
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
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, 0f, 0f);
        }
    }
}
