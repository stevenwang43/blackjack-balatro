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
    public TMP_Text gameResultText;
    public TMP_Text playerScoreText;
    public TMP_Text gameCountText;

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
        playerScoreText.text = gameManager.Score.ToString();
        gameCountText.text = gameManager.playerRoundsWon.ToString() + " / " + gameManager.playerRoundsLost.ToString();
        if (gameManager.state == GameManager.GameState.PlayerTurn)
        {
            hitButton.gameObject.SetActive(true);
            standButton.gameObject.SetActive(true);
            gameResultText.text = "";
        }
        else if (gameManager.state == GameManager.GameState.DealerTurn)
        {
            hitButton.gameObject.SetActive(false);
            standButton.gameObject.SetActive(false);
            gameResultText.text = "Dealer's Turn";
        }
        else if (gameManager.state == GameManager.GameState.RoundWon)
        {
            hitButton.gameObject.SetActive(false);
            standButton.gameObject.SetActive(false);
            gameResultText.text = "You Win!";
        }
        else if (gameManager.state == GameManager.GameState.RoundLost)
        {
            hitButton.gameObject.SetActive(false);
            standButton.gameObject.SetActive(false);
            gameResultText.text = "You Lose!";
        }
    }
}
