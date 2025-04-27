using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Dealer dealer;

    public HandManager playerHand;
    public DeckManager playerDeck;
    public HandManager dealerHand;
    public UIManager uiManager;
    public MainManager manager;
    public ShopManager shopManager;
    public ModifierManager modifierManager;

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

    void Start()
    {
        // Initialize ModifierManager if it doesn't exist
        if (modifierManager == null)
        {
            if (ModifierManager.Instance == null)
            {
                GameObject modObj = new GameObject("ModifierManager");
                modifierManager = modObj.AddComponent<ModifierManager>();
            }
            else
            {
                modifierManager = ModifierManager.Instance;
            }
        }
    }

    public void PlayerDraw()
    {
        Debug.Log("Player Draws Card");
        playerDeck.DrawCard(playerHand);
        uiManager.UpdateGameplayUI();
        
        // Check bust against possibly modified blackjack threshold
        float blackjackThreshold = 21;
        if (modifierManager != null)
        {
            blackjackThreshold = modifierManager.GetModifierValue(ModifierManager.ModifierType.BlackjackThreshold);
        }
        
        if(playerHand.GetTotal() > blackjackThreshold)
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
        
        // Play lose sound effect
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundType.Lose);
        }
        
        // Apply score modifier
        float scoreMultiplier = 1.0f;
        if (modifierManager != null)
        {
            scoreMultiplier = modifierManager.GetModifierValue(ModifierManager.ModifierType.ScoreMultiplier);
        }
        
        Score -= Mathf.RoundToInt(1 * scoreMultiplier);
        uiManager.UpdateGameplayUI();
        
        // Update modifier durations after round end
        if (modifierManager != null)
        {
            modifierManager.UpdateModifierDurations();
        }
        
        yield return new WaitForSeconds(2f);
        ResetGame();
    }

    IEnumerator Win()
    {
        state = GameState.RoundWon;
        
        // Play win sound effect
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundType.Win);
        }
        
        // Apply score modifier
        float scoreMultiplier = 1.0f;
        if (modifierManager != null)
        {
            scoreMultiplier = modifierManager.GetModifierValue(ModifierManager.ModifierType.ScoreMultiplier);
        }
        
        Score += Mathf.RoundToInt(1 * scoreMultiplier);
        dealer.TakeDamage(playerHand.GetValueTotal(), dealer.HandTotal());
        uiManager.UpdateGameplayUI();
        
        // Update modifier durations after round end
        if (modifierManager != null)
        {
            modifierManager.UpdateModifierDurations();
        }

        yield return new WaitForSeconds(2f);
        if (playerRoundsWon == 1) {
            yield return new WaitForSeconds(2f);
            
            // Apply money modifier
            float moneyMultiplier = 1.0f;
            if (modifierManager != null)
            {
                moneyMultiplier = modifierManager.GetModifierValue(ModifierManager.ModifierType.MoneyMultiplier);
            }
            
            // Award money with any applicable multipliers
            int moneyReward = Mathf.RoundToInt(15 * moneyMultiplier);
            shopManager.GainMoney(moneyReward);
            playerHand.ResetHand();
            dealer.ResetHand();
            dealer.maxHealth *= 2;
            dealer.health = dealer.maxHealth;
            manager.setScene(MainManager.SceneState.Shop);
        } else {
            ResetGame();
        }
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
        
        // Reset all modifiers for a new game
        if (modifierManager != null)
        {
            modifierManager.ResetAllModifiers();
        }
        
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
            
            // Get dealer cap modifier if exists
            float dealerCap = 100f; // Default high value (no cap)
            float blackjackThreshold = 21;
            if (modifierManager != null)
            {
                dealerCap = modifierManager.GetModifierValue(ModifierManager.ModifierType.DealerHandleCap);
                blackjackThreshold = modifierManager.GetModifierValue(ModifierManager.ModifierType.BlackjackThreshold);
            }
            
            if (playerHand.GetTotal() <= blackjackThreshold)
            {
                StartCoroutine(Win());
            } else {
                StartCoroutine(Lose());
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
