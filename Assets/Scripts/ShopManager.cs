using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Blackjack;

public class ShopManager : MonoBehaviour {
    public TMP_Text moneyText;
    public Button rerollButton;
    public Button exitButton;
    public Transform shopContentParent;
    public int startingMoney = 10;
    private int currentMoney;
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

        allAvailableCards = Resources.LoadAll<Card>("Standard Deck");

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
        ShopSlot newSlot = Instantiate(shopSlot, shopContentParent);
        newSlot.InitializeSlot(GetRandomCard(), 1, this);
        newSlot.transform.localPosition = new Vector3(350f * (i - 2), 0f, 0f);
        activeSlots.Add(newSlot);
    }
}


    private Card GetRandomCard() {
        int randomIndex = Random.Range(0, allAvailableCards.Length);
        return allAvailableCards[randomIndex];
    }

    public void RerollShop() {
        LoadShopItems();
    }

    public void ExitShop() {
    foreach (ShopSlot slot in activeSlots) {
        if (slot != null)
            Destroy(slot.gameObject);
    }
    activeSlots.Clear();

        mainManager.setScene(MainManager.SceneState.InGame);
        gameManager.StartNewGame();
    }

    public void BuyCard(ShopSlot slot) {
        if (currentMoney >= slot.price) {
            currentMoney -= slot.price;
            deckManager.allCards.Add(slot.card);
            activeSlots.Remove(slot);
            Destroy(slot.gameObject);
            UpdateMoneyUI();
        }
    }

    void UpdateMoneyUI() {
        if (moneyText != null)
            moneyText.text = "$" + currentMoney.ToString();
    }
}
