using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Blackjack;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text valueText;
    public Image[] suitImages;
    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        valueText.text = cardData.value.ToString();

        for (int i = 0; i < suitImages.Length; i++) {
            if (i == cardData.cardSuit) {
                suitImages[i].gameObject.SetActive(true);
            }
        }
    }
}
