using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    public enum CardSuit
    {
        Hearts,
        Tiles,
        Clovers,
        Pikes
    }

    public enum CardRank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack = 10,
        Queen = 10,
        King = 10,
        Ace = 11
    }
    [CreateAssetMenu(fileName = "Card", menuName = "Cards", order = 0)]
    public class CardData : ScriptableObject
    {
        public Sprite sprite;
        public CardSuit suit;
        public CardRank rank;

        public int GetPoints()
        {
            return (int)rank;
        }
    }
}