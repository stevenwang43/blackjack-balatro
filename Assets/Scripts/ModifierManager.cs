using System.Collections.Generic;
using UnityEngine;
using Blackjack;

public class ModifierManager : MonoBehaviour
{
    // Singleton pattern for easy access
    public static ModifierManager Instance { get; private set; }

    // Different modifier types
    public enum ModifierType
    {
        HandTotalBonus,        // Adds to the final hand total
        CardValueMultiplier,   // Multiplies individual card values
        BlackjackThreshold,    // Changes the threshold for blackjack (normally 21)
        ScoreMultiplier,       // Multiplies score gained/lost
        MoneyMultiplier,       // Multiplies money gained from winning
        DealerHandleCap        // Caps the dealer's hand total
    }

    // Dictionary to store active modifiers
    private Dictionary<ModifierType, List<ModifierData>> activeModifiers = new Dictionary<ModifierType, List<ModifierData>>();

    // Structure to hold modifier data
    public class ModifierData
    {
        public string name;
        public float value;
        public int duration; // Number of rounds or -1 for permanent
        public Card sourceCard; // The card that provided this modifier (could be null)

        public ModifierData(string name, float value, int duration = -1, Card sourceCard = null)
        {
            this.name = name;
            this.value = value;
            this.duration = duration;
            this.sourceCard = sourceCard;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeModifiers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeModifiers()
    {
        // Initialize dictionary for all modifier types
        foreach (ModifierType type in System.Enum.GetValues(typeof(ModifierType)))
        {
            activeModifiers[type] = new List<ModifierData>();
        }
    }

    // Add a new modifier
    public void AddModifier(ModifierType type, string name, float value, int duration = -1, Card sourceCard = null)
    {
        activeModifiers[type].Add(new ModifierData(name, value, duration, sourceCard));
        Debug.Log($"Added modifier: {name} of type {type} with value {value}");
    }

    // Remove a modifier by name
    public void RemoveModifier(ModifierType type, string name)
    {
        activeModifiers[type].RemoveAll(m => m.name == name);
    }

    // Remove all modifiers from a specific card
    public void RemoveModifiersFromCard(Card card)
    {
        foreach (var type in activeModifiers.Keys)
        {
            activeModifiers[type].RemoveAll(m => m.sourceCard == card);
        }
    }

    // Decrease duration of temporary modifiers and remove expired ones
    public void UpdateModifierDurations()
    {
        foreach (var type in activeModifiers.Keys)
        {
            for (int i = activeModifiers[type].Count - 1; i >= 0; i--)
            {
                var modifier = activeModifiers[type][i];
                if (modifier.duration > 0)
                {
                    modifier.duration--;
                    if (modifier.duration <= 0)
                    {
                        Debug.Log($"Modifier expired: {modifier.name}");
                        activeModifiers[type].RemoveAt(i);
                    }
                }
            }
        }
    }

    // Calculate the total modifier value for a specific type
    public float GetModifierValue(ModifierType type)
    {
        float total = 0;
        
        switch (type)
        {
            case ModifierType.HandTotalBonus:
            case ModifierType.ScoreMultiplier:
            case ModifierType.MoneyMultiplier:
            case ModifierType.CardValueMultiplier:
                // These add/multiply their values
                foreach (var modifier in activeModifiers[type])
                {
                    total += modifier.value;
                }
                // For multipliers, need to add 1.0 to make it a proper multiplier
                if (type == ModifierType.ScoreMultiplier || 
                    type == ModifierType.MoneyMultiplier ||
                    type == ModifierType.CardValueMultiplier)
                {
                    total += 1.0f;
                }
                break;
                
            case ModifierType.BlackjackThreshold:
                // Default blackjack threshold is 21 if no modifiers
                total = 21;
                foreach (var modifier in activeModifiers[type])
                {
                    total += modifier.value;
                }
                break;
                
            case ModifierType.DealerHandleCap:
                // Default is very high (no cap)
                total = 100;
                // Take the lowest cap if multiple exist
                foreach (var modifier in activeModifiers[type])
                {
                    if (modifier.value < total)
                        total = modifier.value;
                }
                break;
        }
        
        return total;
    }

    // Get all active modifiers of a specific type
    public List<ModifierData> GetActiveModifiers(ModifierType type)
    {
        if (activeModifiers.ContainsKey(type))
            return new List<ModifierData>(activeModifiers[type]);
        return new List<ModifierData>();
    }

    // Clear all modifiers (for new game)
    public void ResetAllModifiers()
    {
        InitializeModifiers();
    }
}