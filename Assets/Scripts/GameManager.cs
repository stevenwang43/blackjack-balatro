using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dealer dealer;

    public HandManager playerHand;
    public DeckManager playerDeck;
    public UIManager uiManager;
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
            return;
        }
    }

    void Lose()
    {

    }



}
