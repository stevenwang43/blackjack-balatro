using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Blackjack;

public class ShopPanelController : MonoBehaviour
{
    public GameObject cardOptionPrefab; // Prefab with Image + Button + maybe price
    public Transform cardOptionParent; // Parent object to hold the options
    public TMP_Text shopTitle;
    public TMP_Text playerMoneyText;

    public void SetupShop(string title, int playerMoney, Card[] availableCards, System.Action<int> onBuy)
    {
        // Clear previous options
        foreach (Transform child in cardOptionParent)
        {
            Destroy(child.gameObject);
        }

        shopTitle.text = title;
        playerMoneyText.text = "Money: $" + playerMoney;

        for (int i = 0; i < availableCards.Length; i++)
        {
            Card card = availableCards[i];
            int index = i; // Capture for closure

            GameObject option = Instantiate(cardOptionPrefab, cardOptionParent);
            Image cardImage = option.GetComponentInChildren<Image>();
            TMP_Text priceText = option.GetComponentInChildren<TMP_Text>();
            Button buyButton = option.GetComponentInChildren<Button>();

            if (cardImage != null && card.cardSprite != null)
            {
                cardImage.sprite = card.cardSprite;
            }

            if (priceText != null)
            {
                priceText.text = "$" + GetCardPrice(card).ToString();
            }

            if (buyButton != null)
            {
                buyButton.onClick.AddListener(() => onBuy(index));
            }
        }
    }

    private int GetCardPrice(Card card)
    {
        if (card.cardType.Contains(Card.CardType.Joker)) return 50;
        if (card.cardType.Contains(Card.CardType.Consumable)) return 20;
        return 10;
    }
}
