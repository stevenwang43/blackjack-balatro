using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Dealer dealer;

    public HandManager playerHand;
    public DeckManager playerDeck;
    public UIManager uiManager;
    public MainManager mainManager;
    int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            uiManager.UpdateGameplayUI();
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
        Debug.Log("Player Draws Card");
        playerDeck.DrawCard(playerHand);
        uiManager.UpdateGameplayUI();
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
        Debug.Log("Player Stands");
        state = GameState.DealerTurn;
        StartCoroutine(DealerTurnWithDelay(true));
    }

    IEnumerator Lose()
    {
        state = GameState.RoundLost;
        playerRoundsLost++;
        uiManager.UpdateGameplayUI();
        yield return new WaitForSeconds(2f);
        ResetGame();
    }

    IEnumerator Win()
    {
        state = GameState.RoundWon;
        playerRoundsWon++;
        Score += playerHand.GetTotal();
        if (playerHand.GetTotal() == 21)
        {
            Score += playerHand.GetTotal();
        }
        uiManager.UpdateGameplayUI();
        yield return new WaitForSeconds(2f);
        if(playerRoundsWon >= 1)
        {
            GameWin();
            yield break;
        }
        if (playerRoundsLost >= 5)
        {
            GameLose();
            yield break;
        }
        ResetGame();
    }

    public void GameWin()
    {
        Debug.Log("Game Won");
        uiManager.UpdateGameplayUI();
        StartCoroutine(mainManager.WinRound()); 
    }
    public void GameLose()
    {
        Debug.Log("Game Lost");
        uiManager.UpdateGameplayUI();
        mainManager.setScene(MainManager.SceneState.StartMenu);
    }

    public void ResetGame()
    {
        playerHand.ResetHand();
        dealer.ResetHand();
        state = GameState.PlayerTurn;
        uiManager.UpdateGameplayUI();
    }

    public void StartNewGame()
    {
        playerHand.ResetHand();
        dealer.ResetHand();
        playerDeck.ResetDeck();
        playerRoundsWon = 0;
        playerRoundsLost = 0;
        Score = 0;
        state = GameState.PlayerTurn;
        uiManager.StartGame();
        uiManager.UpdateGameplayUI();
    }

    IEnumerator DealerTurnWithDelay(bool fullTurn)
    {
        yield return new WaitForSeconds(0.5f); // delay before starting

        if (fullTurn)
        {
            int result = dealer.TakeTurn();
            uiManager.UpdateGameplayUI();
            while (result == 1)
            {
                yield return new WaitForSeconds(0.5f); // delay between dealer actions
                result = dealer.TakeTurn();
                uiManager.UpdateGameplayUI();
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
            uiManager.UpdateGameplayUI();
            state = GameState.PlayerTurn;
        }
    }




}
