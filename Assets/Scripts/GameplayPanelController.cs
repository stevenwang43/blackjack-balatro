using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayPanelController : MonoBehaviour
{
    public Button hitButton;
    public Button standButton;
    public TMP_Text playerHandTotal;
    public TMP_Text dealerHandTotal;
    public TMP_Text gameResultText;
    public TMP_Text playerScoreText;
    public TMP_Text gameCountText;
    public TMP_Text deckText;

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

    public void UpdateHandTotals(int playerTotal, int dealerTotal)
    {
        playerHandTotal.text = playerTotal.ToString();
        dealerHandTotal.text = dealerTotal.ToString();
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

    public void SetGameStateUI(string resultText, bool showButtons)
    {
        hitButton.gameObject.SetActive(showButtons);
        standButton.gameObject.SetActive(showButtons);
        gameResultText.text = resultText;
    }
}
