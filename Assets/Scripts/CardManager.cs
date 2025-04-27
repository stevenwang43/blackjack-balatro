using UnityEngine;
using UnityEditor;
using System.IO;
using Blackjack;

#if UNITY_EDITOR
public class CardGenerator : MonoBehaviour
{
    [MenuItem("Tools/Create Modifier Cards")]
    public static void CreateModifierCards()
    {
        /*
        Script to create 6 example AI generated modifier cards with unique effects
        Can click the button and edit the card properties in the inspector
        */
        // Create directory if it doesn't exist
        string folderPath = "Assets/Resources/Special Cards";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        // Create Ace of Fate (Blackjack threshold increase)
        Card aceOfFate = ScriptableObject.CreateInstance<Card>();
        aceOfFate.value = 11;
        aceOfFate.cardSuit = 0; // Spades
        aceOfFate.displayValue = "A";
        aceOfFate.cardType = new System.Collections.Generic.List<Card.CardType>{ Card.CardType.Joker };
        
        aceOfFate.hasModifier = true;
        aceOfFate.modifierName = "Fate's Blessing";
        aceOfFate.modifierDescription = "Increases blackjack threshold by 1";
        aceOfFate.modifierEffect = Card.ModifierEffect.BlackjackThresholdIncrease;
        aceOfFate.modifierValue = 1.0f;
        aceOfFate.modifierDuration = -1; // Permanent
        
        AssetDatabase.CreateAsset(aceOfFate, $"{folderPath}/AceOfFate.asset");

        // Create Diamond Joker (Money multiplier)
        Card diamondJoker = ScriptableObject.CreateInstance<Card>();
        diamondJoker.value = 0;
        diamondJoker.cardSuit = 2; // Diamonds
        diamondJoker.displayValue = "J";
        diamondJoker.cardType = new System.Collections.Generic.List<Card.CardType>{ Card.CardType.Joker };
        
        diamondJoker.hasModifier = true;
        diamondJoker.modifierName = "Diamond Fortune";
        diamondJoker.modifierDescription = "Increases money rewards by 50%";
        diamondJoker.modifierEffect = Card.ModifierEffect.MoneyMultiplier;
        diamondJoker.modifierValue = 0.5f;
        diamondJoker.modifierDuration = -1; // Permanent
        
        AssetDatabase.CreateAsset(diamondJoker, $"{folderPath}/DiamondJoker.asset");

        // Create Heart Queen (Hand total bonus)
        Card heartQueen = ScriptableObject.CreateInstance<Card>();
        heartQueen.value = 12;
        heartQueen.cardSuit = 1; // Hearts
        heartQueen.displayValue = "Q";
        heartQueen.cardType = new System.Collections.Generic.List<Card.CardType>{ Card.CardType.Joker };
        
        heartQueen.hasModifier = true;
        heartQueen.modifierName = "Queen's Grace";
        heartQueen.modifierDescription = "Adds +2 to your hand total";
        heartQueen.modifierEffect = Card.ModifierEffect.HandTotalBonus;
        heartQueen.modifierValue = 2.0f;
        heartQueen.modifierDuration = -1; // Permanent
        
        AssetDatabase.CreateAsset(heartQueen, $"{folderPath}/HeartQueen.asset");

        // Create Club King (Dealer hand cap)
        Card clubKing = ScriptableObject.CreateInstance<Card>();
        clubKing.value = 13;
        clubKing.cardSuit = 3; // Clubs
        clubKing.displayValue = "K";
        clubKing.cardType = new System.Collections.Generic.List<Card.CardType>{ Card.CardType.Joker };
        
        clubKing.hasModifier = true;
        clubKing.modifierName = "Royal Decree";
        clubKing.modifierDescription = "Caps dealer's hand at 18";
        clubKing.modifierEffect = Card.ModifierEffect.DealerHandleCap;
        clubKing.modifierValue = 18.0f;
        clubKing.modifierDuration = -1; // Permanent
        
        AssetDatabase.CreateAsset(clubKing, $"{folderPath}/ClubKing.asset");

        // Create Lucky Spade (Score multiplier)
        Card luckySpade = ScriptableObject.CreateInstance<Card>();
        luckySpade.value = 7;
        luckySpade.cardSuit = 0; // Spades
        luckySpade.displayValue = "7";
        luckySpade.cardType = new System.Collections.Generic.List<Card.CardType>{ Card.CardType.Joker };
        
        luckySpade.hasModifier = true;
        luckySpade.modifierName = "Lady Luck";
        luckySpade.modifierDescription = "Increases score by 25%";
        luckySpade.modifierEffect = Card.ModifierEffect.ScoreMultiplier;
        luckySpade.modifierValue = 0.25f;
        luckySpade.modifierDuration = -1; // Permanent
        
        AssetDatabase.CreateAsset(luckySpade, $"{folderPath}/LuckySpade.asset");

        // Temporary Card Value Multiplier (3 rounds)
        Card valueBooster = ScriptableObject.CreateInstance<Card>();
        valueBooster.value = 3;
        valueBooster.cardSuit = 2; // Diamonds
        valueBooster.displayValue = "3";
        valueBooster.cardType = new System.Collections.Generic.List<Card.CardType>{ Card.CardType.Consumable };
        
        valueBooster.hasModifier = true;
        valueBooster.modifierName = "Value Boost";
        valueBooster.modifierDescription = "Cards worth 20% more for 3 rounds";
        valueBooster.modifierEffect = Card.ModifierEffect.CardValueMultiplier;
        valueBooster.modifierValue = 0.2f;
        valueBooster.modifierDuration = 3; // Temporary - 3 rounds
        
        AssetDatabase.CreateAsset(valueBooster, $"{folderPath}/ValueBooster.asset");
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Created 6 modifier cards in " + folderPath);
    }
}
#endif