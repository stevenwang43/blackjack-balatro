using UnityEngine;
using UnityEngine.UI;
using Blackjack;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        cardImage.sprite = cardData.cardSprite;
    }
}
