using UnityEngine;

public class Dealer : MonoBehaviour
{
    public DeckManager deck;
    public HandManager hand;
    public GameManager gameManager;
    public GameplayPanelController gameplayPanelController;
    public double health;
    public double maxHealth = 20;
    public int damageTaken;

    void Start()
    {
        health = maxHealth;
        gameplayPanelController.UpdateDealerHealthText((int)health, (int)maxHealth);
    }

    //return 0 on stand, 1 on hit
    public int TakeTurn()
    {
        // Get dealer threshold from modifiers if they exist
        float dealerStandThreshold = 17f; // Default dealer stands on 17
        
        // If a DealerHandleCap modifier exists, dealer will stand at a lower value
        if (ModifierManager.Instance != null)
        {
            float dealerCap = ModifierManager.Instance.GetModifierValue(ModifierManager.ModifierType.DealerHandleCap);
            if (dealerCap < 21)
            {
                // If there's a cap below 21, adjust dealer behavior to avoid exceeding it
                dealerStandThreshold = Mathf.Max(10, dealerCap - 3);
            }
        }
        
        if(hand.GetTotal() < dealerStandThreshold)
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

    public int HandValueTotal()
    {
        return hand.GetValueTotal();
    }

    public void ResetHand()
    {
        hand.ResetHand();
    }

    public void TakeDamage(int damage, int block, int dealerDraw) {
        if (dealerDraw > 21) {
            block = 0;
        }
        damageTaken = damage - block;
        health = (int)health - Mathf.Max(damageTaken, 0);
        if (health <= 0) {
            gameManager.playerRoundsWon += 1;
        }
        gameplayPanelController.UpdateDealerHealthText((int)health, (int)maxHealth);
    }
}
