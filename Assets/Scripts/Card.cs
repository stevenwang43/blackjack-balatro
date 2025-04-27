using System.Collections.Generic;
using UnityEngine;

namespace Blackjack 
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject 
    {
        public int value;
        public int cardSuit;
        public string displayValue = "";
        public List<CardType> cardType;

        public Sprite cardSprite;

        // New fields for modifiers
        public bool hasModifier = false;
        public string modifierName = "";
        public string modifierDescription = "";
        [Tooltip("The type of modifier this card applies")]
        public ModifierEffect modifierEffect = ModifierEffect.None;
        [Tooltip("The value of the modifier effect")]
        public float modifierValue = 0f;
        [Tooltip("Duration of the modifier in rounds, -1 for permanent")]
        public int modifierDuration = -1;

        public enum CardType
        {
            Playing,
            Consumable,
            Joker
        }

        // Types of modifier effects a card can have
        public enum ModifierEffect
        {
            None,
            HandTotalBonus,
            CardValueMultiplier,
            BlackjackThresholdIncrease,
            ScoreMultiplier,
            MoneyMultiplier,
            DealerHandleCap
        }

        // Apply the card's modifier when acquired
        public void ApplyModifier()
        {
            if (!hasModifier || ModifierManager.Instance == null)
                return;

            ModifierManager.ModifierType modType = ConvertToModifierType(modifierEffect);
            ModifierManager.Instance.AddModifier(modType, modifierName, modifierValue, modifierDuration, this);
        }

        // Convert from the card's ModifierEffect to ModifierManager.ModifierType
        private ModifierManager.ModifierType ConvertToModifierType(ModifierEffect effect)
        {
            switch (effect)
            {
                case ModifierEffect.HandTotalBonus:
                    return ModifierManager.ModifierType.HandTotalBonus;
                case ModifierEffect.CardValueMultiplier:
                    return ModifierManager.ModifierType.CardValueMultiplier;
                case ModifierEffect.BlackjackThresholdIncrease:
                    return ModifierManager.ModifierType.BlackjackThreshold;
                case ModifierEffect.ScoreMultiplier:
                    return ModifierManager.ModifierType.ScoreMultiplier;
                case ModifierEffect.MoneyMultiplier:
                    return ModifierManager.ModifierType.MoneyMultiplier;
                case ModifierEffect.DealerHandleCap:
                    return ModifierManager.ModifierType.DealerHandleCap;
                default:
                    return ModifierManager.ModifierType.HandTotalBonus;
            }
        }
    }
}