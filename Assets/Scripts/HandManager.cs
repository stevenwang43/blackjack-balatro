using UnityEngine;
using Blackjack;
using System.Collections.Generic;

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
            Vector3 newPosition = new Vector3(horizontalOffset, 0f, 0f);
            cardsInHand[i].GetComponent<DragUIObject>().SetPosition(newPosition);
        }
    }

    public void ReorderCards()
    {
        cardsInHand.Sort((a, b) => a.transform.localPosition.x.CompareTo(b.transform.localPosition.x));

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            float horizontalOffset = i * cardSpacing;
            Vector3 newPosition = new Vector3(horizontalOffset, 0f, 0f);
            cardsInHand[i].GetComponent<DragUIObject>().SetPosition(newPosition);
        }
    }

}
