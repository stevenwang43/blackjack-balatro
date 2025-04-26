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

    public int GetTotal()
    {
        int total = 0;
        foreach (Card c in cardsData)
        {
            total += c.value;
        }
        return total;

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
