using System.Collections.Generic;
using UnityEngine;

namespace Blackjack 
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject 
    {
        public int value;
        public int cardSuit;
        public List<CardType> cardType;

        public Sprite cardSprite;

        public enum CardType
        {
            Playing,
            Consumable,
            Joker
        }
    }
}