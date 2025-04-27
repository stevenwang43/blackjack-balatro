using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Blackjack;

public class ShopSlot : MonoBehaviour, IPointerClickHandler {
    public TMP_Text priceText;
    public GameObject cardPrefab;

    [HideInInspector]
    public int price;
    [HideInInspector]
    public Card card;
    
    private ShopManager shopManager;

    public void InitializeSlot(Card card, int price, ShopManager manager) {
        this.card = card;
        this.price = price;
        shopManager = manager;

        if (cardPrefab != null) {
            GameObject newCardObject = Instantiate(cardPrefab, transform);
            CardDisplay cardDisplay = newCardObject.GetComponent<CardDisplay>();

            if (cardDisplay != null) {
                cardDisplay.cardData = card;
                cardDisplay.UpdateCardDisplay();
            }
            
        }

        if (priceText != null)
            priceText.text = "$" + price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData) {
        shopManager.BuyCard(this);
    }
}
