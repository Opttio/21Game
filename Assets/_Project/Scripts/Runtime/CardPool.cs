using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class CardPool : MonoBehaviour
    {
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private Transform _cardStackPosition;
        [SerializeField] private List<CardData> allCards;
        private CardView _cardView;
        private readonly Queue<CardView> _cardQueue = new Queue<CardView>();
        
        private DiContainer _container;
        
        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        
        private void Start()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            var shuffledCards = new List<CardData>(allCards);

            // Алгоритм Фішера-Йєйтса
            System.Random rng = new System.Random();
            int n = shuffledCards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (shuffledCards[k], shuffledCards[n]) = (shuffledCards[n], shuffledCards[k]);
            }

            foreach (var card in shuffledCards)
            {
                var cardGO = _container.InstantiatePrefab(_cardPrefab, _cardStackPosition.position, _cardStackPosition.rotation, _cardStackPosition);
                var cardView = cardGO.GetComponent<CardView>();
                cardView.SetCard(card);
                _cardQueue.Enqueue(cardView);
                cardGO.SetActive(false);
            }
        }

        public CardView GetNextCard()
        {
            if (_cardQueue.Count == 0)
            {
                Debug.LogWarning("Deck is empty!");
                return null;
            }

            var card = _cardQueue.Dequeue();
            card.gameObject.SetActive(true);
            return card;
        }
        
        public void ReturnCard(CardView card)
        {
            if (card == null) return;

            card.gameObject.SetActive(false);
            card.transform.position = _cardStackPosition.position;
            card.transform.rotation = _cardStackPosition.rotation;

            _cardQueue.Enqueue(card);
        }
    }
}