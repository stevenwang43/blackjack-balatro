using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Dealer dealer;

    public HandManager playerHand;
    public DeckManager playerDeck;
    public UIManager uiManager;
    int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            uiManager.UpdateUI();
        }
    }
    public int playerRoundsWon = 0;
    public int playerRoundsLost = 0;
    public enum GameState
    {
        PlayerTurn,
        DealerTurn,
        RoundWon,
        RoundLost
    }
    public GameState state = GameState.PlayerTurn;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDraw()
    {
        playerDeck.DrawCard(playerHand);
        uiManager.UpdateUI();
        if(playerHand.GetTotal() > 21)
        {
            StartCoroutine(Lose());
            return;
        }
        state = GameState.DealerTurn;
        StartCoroutine(DealerTurnWithDelay(false));
        state = GameState.PlayerTurn;

    }

    public void PlayerStand()
    {
        state = GameState.DealerTurn;
        StartCoroutine(DealerTurnWithDelay(true));
    }

    IEnumerator Lose()
    {
        state = GameState.RoundLost;
        playerRoundsLost++;
        Score -= 1;
        uiManager.UpdateUI();
        yield return new WaitForSeconds(2f);
        ResetGame();
    }

    IEnumerator Win()
    {
        state = GameState.RoundWon;
        playerRoundsWon++;
        Score += 1;
        uiManager.UpdateUI();
        yield return new WaitForSeconds(2f);
        ResetGame();
    }
    void ResetGame()
    {
        playerHand.ResetHand();
        dealer.ResetHand();
        state = GameState.PlayerTurn;
        uiManager.UpdateUI();
    }

    IEnumerator DealerTurnWithDelay(bool fullTurn)
    {
        yield return new WaitForSeconds(0.5f); // delay before starting

        if (fullTurn)
        {
            int result = dealer.TakeTurn();
            uiManager.UpdateUI();
            while (result == 1)
            {
                yield return new WaitForSeconds(0.5f); // delay between dealer actions
                result = dealer.TakeTurn();
                uiManager.UpdateUI();
            }
            if(dealer.HandTotal() > 21)
            {
                StartCoroutine(Win());
            }
            else if (dealer.HandTotal() >= playerHand.GetTotal())
            {
                StartCoroutine(Lose()); // dealer wins
            }
            else
            {
                StartCoroutine(Win()); // player wins
            }
        }
        else
        {
            dealer.TakeTurn(); // just one action
            uiManager.UpdateUI();
            state = GameState.PlayerTurn;
        }
    }




}
