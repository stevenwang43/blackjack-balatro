using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifierUIManager : MonoBehaviour
{
    public GameObject modifierPanel;
    public GameObject modifierEntryPrefab;
    public Transform modifierListContent;
    
    private Dictionary<string, GameObject> activeModifierUI = new Dictionary<string, GameObject>();
    
    void Start()
    {
        // Hide panel at start if it's empty
        UpdateModifierDisplay();
    }
    
    void Update()
    {
        // Update the display every frame to catch any changes
        // In a real game you might want to only update this when modifiers change
        UpdateModifierDisplay();
    }
    
    void UpdateModifierDisplay()
    {
        if (ModifierManager.Instance == null)
        {
            if (modifierPanel != null)
                modifierPanel.SetActive(false);
            return;
        }
        
        // Get all active modifiers from the manager
        List<ModifierInfoDisplay> activeModifiers = new List<ModifierInfoDisplay>();
        
        foreach (ModifierManager.ModifierType type in System.Enum.GetValues(typeof(ModifierManager.ModifierType)))
        {
            var modifiers = ModifierManager.Instance.GetActiveModifiers(type);
            foreach (var modifier in modifiers)
            {
                string displayValue = GetDisplayValue(type, modifier.value);
                string suffix = (modifier.duration > 0) ? $" ({modifier.duration} rounds left)" : "";
                
                activeModifiers.Add(new ModifierInfoDisplay(
                    modifier.name,
                    $"{GetTypePrefix(type)} {displayValue}{suffix}",
                    GetColorForModifierType(type)
                ));
            }
        }
        
        // Update the UI
        // First, remove entries that aren't active anymore
        List<string> keysToRemove = new List<string>();
        foreach (var key in activeModifierUI.Keys)
        {
            bool stillActive = false;
            foreach (var modifier in activeModifiers)
            {
                if (modifier.name == key)
                {
                    stillActive = true;
                    break;
                }
            }
            if (!stillActive)
                keysToRemove.Add(key);
        }
        
        foreach (var key in keysToRemove)
        {
            Destroy(activeModifierUI[key]);
            activeModifierUI.Remove(key);
        }
        
        // Then add or update entries
        foreach (var modifier in activeModifiers)
        {
            if (activeModifierUI.ContainsKey(modifier.name))
            {
                // Update existing entry
                var textComponent = activeModifierUI[modifier.name].GetComponentInChildren<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = $"{modifier.name}: {modifier.description}";
            }
            else
            {
                // Create new entry
                GameObject newEntry = Instantiate(modifierEntryPrefab, modifierListContent);
                var textComponent = newEntry.GetComponentInChildren<TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.text = $"{modifier.name}: {modifier.description}";
                    textComponent.color = modifier.color;
                }
                activeModifierUI[modifier.name] = newEntry;
            }
        }
        
        // Show or hide the panel based on whether there are active modifiers
        if (modifierPanel != null)
            modifierPanel.SetActive(activeModifiers.Count > 0);
    }
    
    private string GetDisplayValue(ModifierManager.ModifierType type, float value)
    {
        switch (type)
        {
            case ModifierManager.ModifierType.HandTotalBonus:
                return $"+{value}";
            case ModifierManager.ModifierType.CardValueMultiplier:
            case ModifierManager.ModifierType.ScoreMultiplier:
            case ModifierManager.ModifierType.MoneyMultiplier:
                return $"+{value*100}%";
            case ModifierManager.ModifierType.BlackjackThreshold:
                return value > 0 ? $"+{value}" : $"{value}";
            case ModifierManager.ModifierType.DealerHandleCap:
                return $"{value}";
            default:
                return value.ToString();
        }
    }
    
    private string GetTypePrefix(ModifierManager.ModifierType type)
    {
        switch (type)
        {
            case ModifierManager.ModifierType.HandTotalBonus:
                return "Hand Bonus:";
            case ModifierManager.ModifierType.CardValueMultiplier:
                return "Card Value:";
            case ModifierManager.ModifierType.BlackjackThreshold:
                return "Blackjack Threshold:";
            case ModifierManager.ModifierType.ScoreMultiplier:
                return "Score:";
            case ModifierManager.ModifierType.MoneyMultiplier:
                return "Money:";
            case ModifierManager.ModifierType.DealerHandleCap:
                return "Dealer Cap:";
            default:
                return "";
        }
    }
    
    private Color GetColorForModifierType(ModifierManager.ModifierType type)
    {
        switch (type)
        {
            case ModifierManager.ModifierType.HandTotalBonus:
                return new Color(0.2f, 0.8f, 0.2f); // Green
            case ModifierManager.ModifierType.CardValueMultiplier:
                return new Color(0.8f, 0.6f, 0.0f); // Gold
            case ModifierManager.ModifierType.BlackjackThreshold:
                return new Color(0.2f, 0.4f, 0.8f); // Blue
            case ModifierManager.ModifierType.ScoreMultiplier:
                return new Color(0.8f, 0.2f, 0.8f); // Purple
            case ModifierManager.ModifierType.MoneyMultiplier:
                return new Color(0.2f, 0.8f, 0.8f); // Cyan
            case ModifierManager.ModifierType.DealerHandleCap:
                return new Color(0.8f, 0.2f, 0.2f); // Red
            default:
                return Color.white;
        }
    }
    
    private class ModifierInfoDisplay
    {
        public string name;
        public string description;
        public Color color;
        
        public ModifierInfoDisplay(string name, string description, Color color)
        {
            this.name = name;
            this.description = description;
            this.color = color;
        }
    }
}