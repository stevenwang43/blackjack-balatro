using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button hitButton;
    public Button standButton;
    public TMP_Text playerHandTotal;
    public TMP_Text dealerHandTotal;
    public HandManager playerHand;
    public GameManager gameManager;
    public Dealer dealer;

    void Start()
    {
        hitButton.onClick.AddListener(() => gameManager.PlayerDraw());
        standButton.onClick.AddListener(() => gameManager.PlayerStand());
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerHandTotal.text = playerHand.GetTotal().ToString();
        dealerHandTotal.text = dealer.HandTotal().ToString();

    }
}
