using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Dealer dealer;

    public HandManager playerHand;
    public DeckManager playerDeck;
    public UIManager uiManager;
    public enum GameState
    {
        PlayerTurn,
        DealerTurn,
        RoundOver
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
            state = GameState.RoundOver;
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

    void Lose()
    {

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
            state = GameState.RoundOver;
        }
        else
        {
            dealer.TakeTurn(); // just one action
            uiManager.UpdateUI();
            state = GameState.PlayerTurn;
        }
    }




}
