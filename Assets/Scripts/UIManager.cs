using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button drawCardButton;
    public TMP_Text playerHandTotal;
    public HandManager playerHand;
    public GameManager gameManager;

    void Start()
    {
        drawCardButton.onClick.AddListener(() => gameManager.PlayerDraw());
    }

    public void UpdateUI()
    {
        playerHandTotal.text = playerHand.GetTotal().ToString();
    }
}
