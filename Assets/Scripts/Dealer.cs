using UnityEngine;

public class Dealer : MonoBehaviour
{

    public DeckManager deck;
    public HandManager hand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //return 0 on stand, 1 on hit
    public int TakeTurn()
    {
        if(hand.GetTotal() < 17)
        {
            deck.DrawCard(hand);
            return 1;
        }
        return 0;
    }

    public int HandTotal()
    {
        return hand.GetTotal();
    }


}
