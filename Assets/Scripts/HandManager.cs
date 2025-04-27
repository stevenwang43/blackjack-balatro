using UnityEngine;
using Blackjack;
using System.Collections.Generic;
using System;

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
        float total = 0;
        
        // Get card value multiplier if ModifierManager exists
        float cardValueMultiplier = 1.0f;
        if (ModifierManager.Instance != null)
        {
            cardValueMultiplier = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.CardValueMultiplier);
        }
        
        foreach (Card c in cardsData)
        {
            total += c.value * cardValueMultiplier;
        }
        
        // Apply hand total bonus if ModifierManager exists
        if (ModifierManager.Instance != null)
        {
            total += ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.HandTotalBonus);
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
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, 0f, 0f);
        }
    }
}
