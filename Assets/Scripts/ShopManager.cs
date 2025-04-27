using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Blackjack;

public class ShopManager : MonoBehaviour {
    public TMP_Text moneyText;
    public TMP_Text rerollText;
    public Button rerollButton;
    public Button exitButton;
    public Transform shopContentParent;
    public int startingMoney = 0;
    private int currentMoney;
    public float rerollCost = 2f;
    public int rerollCount = 0;
    public int shopSlotCount = 5;
    public ShopSlot shopSlot;

    public DeckManager deckManager;
    public MainManager mainManager;
    public GameManager gameManager;

    private Card[] allAvailableCards;
    private List<ShopSlot> activeSlots = new List<ShopSlot>();

    void Start() {
        currentMoney = startingMoney;
        UpdateMoneyUI();
        UpdateRerollUI();

        // Load cards from both Standard Deck and Special Cards folders
        Card[] standardCards = Resources.LoadAll<Card>("Standard Deck");
        Card[] specialCards = Resources.LoadAll<Card>("Special Cards");
        
        // Combine the arrays
        List<Card> combinedCards = new List<Card>();
        // if (standardCards != null && standardCards.Length > 0)
        //     combinedCards.AddRange(standardCards);
        if (specialCards != null && specialCards.Length > 0)
            combinedCards.AddRange(specialCards);
            
        allAvailableCards = combinedCards.ToArray();
        Debug.Log($"Loaded {allAvailableCards.Length} cards into shop ({standardCards?.Length ?? 0} standard, {specialCards?.Length ?? 0} special)");

        if (rerollButton != null)
            rerollButton.onClick.AddListener(RerollShop);
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitShop);

        LoadShopItems();
    }

    void LoadShopItems() {
        foreach (ShopSlot slot in activeSlots) {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        activeSlots.Clear();

        float spacing = 250f;
        float startX = -((shopSlotCount - 1) * spacing) / 2;

        for (int i = 0; i < shopSlotCount; i++) {
            Card randomCard = GetRandomCard();
            int price = 1; // Default price
            
            // Set higher prices for cards with modifiers
            if (randomCard.hasModifier) {
                // Base price for modifier cards
                price = 5;
                
                // Add price based on modifier type and value
                switch (randomCard.modifierEffect) {
                    case Card.ModifierEffect.BlackjackThresholdIncrease:
                        price += Mathf.RoundToInt(10 * randomCard.modifierValue); // Very valuable
                        break;
                    case Card.ModifierEffect.HandTotalBonus:
                        price += Mathf.RoundToInt(5 * randomCard.modifierValue);
                        break;
                    case Card.ModifierEffect.CardValueMultiplier:
                        price += Mathf.RoundToInt(15 * randomCard.modifierValue);
                        break;
                    case Card.ModifierEffect.ScoreMultiplier:
                        price += Mathf.RoundToInt(8 * randomCard.modifierValue);
                        break;
                    case Card.ModifierEffect.MoneyMultiplier:
                        price += Mathf.RoundToInt(12 * randomCard.modifierValue);
                        break;
                    case Card.ModifierEffect.DealerHandleCap:
                        // Lower cap = higher price (21 - cap = price addition)
                        price += Mathf.RoundToInt(21 - randomCard.modifierValue) * 3;
                        break;
                }
                
                // Permanent modifiers are more expensive than temporary ones
                if (randomCard.modifierDuration < 0) {
                    price = Mathf.RoundToInt(price * 1.5f);
                }
            }
            
            ShopSlot newSlot = Instantiate(shopSlot, shopContentParent);
            newSlot.InitializeSlot(randomCard, price, this);
            newSlot.transform.localPosition = new Vector3(350f * (i - 2), 0f, 0f);
            activeSlots.Add(newSlot);
        }
    }

    private Card GetRandomCard() {
        int randomIndex = Random.Range(0, allAvailableCards.Length);
        return allAvailableCards[randomIndex];
    }

    public void RerollShop() {
        if (currentMoney >= (int)rerollCost) {
            currentMoney -= (int)rerollCost;
            rerollCost *= 1.15f;
            
            // Play button click sound
            if (AudioManager.Instance != null) {
                AudioManager.Instance.PlaySound(AudioManager.SoundType.ButtonClick);
            }
            
            UpdateMoneyUI();
            UpdateRerollUI();
            LoadShopItems();
        }
    }

    public void ExitShop() {
        // Play button click sound
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlaySound(AudioManager.SoundType.ButtonClick);
        }

        foreach (ShopSlot slot in activeSlots) {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        activeSlots.Clear();

        mainManager.setScene(MainManager.SceneState.InGame);
        gameManager.StartNewGame();
        rerollCount = 0;
        rerollCost = 2f;
    }

    public void BuyCard(ShopSlot slot) {
        if (currentMoney >= slot.price) {
            currentMoney -= slot.price;
            deckManager.allCards.Add(slot.card);
            
            // Play appropriate sound effect
            if (AudioManager.Instance != null) {
                AudioManager.Instance.PlaySound(AudioManager.SoundType.CardFlip);
                
            }
            
            // Apply card modifiers if it has any
            if (slot.card.hasModifier) {
                slot.card.ApplyModifier();
                Debug.Log($"Applied modifier: {slot.card.modifierName} from {slot.card.name}");
            }
            
            activeSlots.Remove(slot);
            Destroy(slot.gameObject);
            UpdateMoneyUI();
            UpdateRerollUI();
        }
        else {
            AudioManager.Instance?.PlaySound(AudioManager.SoundType.Denied);
            Debug.LogWarning($"Not enough money to buy {slot.card.name}. Current money: {currentMoney}, Price: {slot.price}");
        }
    }

    public void GainMoney(int money) {
        currentMoney += money;
        UpdateMoneyUI();
        
        // Play money sound effect
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MoneyGain);
        }
    }

    void UpdateMoneyUI() {
        if (moneyText != null)
            moneyText.text = "$" + currentMoney.ToString();
    }

    void UpdateRerollUI() {
        if (rerollText != null)
            rerollText.text = "$" + ((int)rerollCost).ToString();
    }
}
