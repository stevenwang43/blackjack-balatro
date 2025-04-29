using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayPanelController : MonoBehaviour
{
    public Button hitButton;
    public Button standButton;
    public TMP_Text playerHandTotal;
    public TMP_Text playerHandValueTotal;
    public TMP_Text dealerHandTotal;
    public TMP_Text dealerHandValueTotal;
    public TMP_Text dealerHealthText;
    public TMP_Text gameResultText;
    public TMP_Text playerScoreText;
    
    public TMP_Text gameCountText;
    public TMP_Text deckText;
    public TMP_Text roundText;

    public void SetupButtons(System.Action onHit, System.Action onStand)
    {
        hitButton.onClick.RemoveAllListeners();
        standButton.onClick.RemoveAllListeners();
        hitButton.onClick.AddListener(() => onHit());
        standButton.onClick.AddListener(() => 
        {
            Debug.Log("Player Stand button clicked");
            onStand();
        });
    }

    public void UpdateHandTotals(int playerTotal, int playerValueTotal, int dealerTotal, int dealerValueTotal)
    {
        playerHandTotal.text = playerTotal.ToString();
        playerHandValueTotal.text = "Damage: " + playerValueTotal.ToString();
        dealerHandTotal.text = dealerTotal.ToString();
        dealerHandValueTotal.text = "Block: " + dealerValueTotal.ToString();

        if (playerTotal == 21) {
            playerHandTotal.color = new Color(1f, 0.84f, 0f);
            playerHandValueTotal.text = "Damage: " + (playerValueTotal / 2).ToString() + " x 2";
        } else if (playerTotal > 21) {
            playerHandTotal.color = Color.red;
        } else {
            playerHandTotal.color = Color.white;
        }
        if (dealerTotal == 21) {
            dealerHandTotal.color = new Color(1f, 0.84f, 0f);
            dealerHandValueTotal.text = "Block: " + (dealerValueTotal / 2).ToString() + " x 2";
        } else if (dealerTotal > 21) {
            dealerHandTotal.color = Color.red;
        } else {
            dealerHandTotal.color = Color.white;
        }
    }

    public void UpdateScore(int score)
    {
        playerScoreText.text = score.ToString();
    }

    public void UpdateGameCount(int wins, int losses)
    {
        gameCountText.text = $"{wins} / {losses}";
    }

    public void UpdateDeckText(int remaining, int total)
    {
        deckText.text = $"{remaining} / {total}";
    }

    public void UpdateDealerHealthText(int remaining, int total)
    {
        dealerHealthText.text = $"{remaining} / {total}";
    }

    public void UpdateRoundText(int round)
    {
        roundText.text = "Round " + round;
    }

    public void SetGameStateUI(string resultText, bool showButtons)
    {
        hitButton.gameObject.SetActive(showButtons);
        standButton.gameObject.SetActive(showButtons);
        gameResultText.text = resultText;
    }
}
