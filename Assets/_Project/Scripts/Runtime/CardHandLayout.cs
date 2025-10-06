using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;
using DG.Tweening;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class CardHandLayout : MonoBehaviour
    {
        [SerializeField] private Transform _handCenter;
        [SerializeField] private float _spacing = 2f;
        [SerializeField] private float _moveDuration = 1f;
        [SerializeField] private bool _updateUI = true;
        
        private readonly List<CardView> _handCards = new List<CardView>();
        private int _points = 0;
        
        public int TotalPoints => _points;
        
        private GameEventBus _eventBus;

        [Inject]
        private void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Start()
        {
            if(_updateUI)
                _eventBus.ChangePoints(_points);
        }

        public void AddCard(CardView card)
        {
            _handCards.Add(card);
            _points += card.CardValue;
            UpdateLayout();
            if(_updateUI)
                _eventBus.ChangePoints(_points);
        }
        
        public void ReturnAllToPool(CardPool pool)
        {
            foreach (var card in _handCards)
            {
                pool.ReturnCard(card);
            }

            _handCards.Clear();
            _points = 0;

            if (_updateUI)
                _eventBus.ChangePoints(_points);
        }

        private void UpdateLayout()
        {
            int count = _handCards.Count;
            if (count == 0) return;

            float totalWidth = (count - 1) * _spacing;
            float startX = -totalWidth / 2f;

            for (int i = 0; i < count; i++)
            {
                Vector3 targetPos = _handCenter.position + new Vector3(startX + i * _spacing, 0, 0);
            
                _handCards[i].transform
                    .DOMove(targetPos, _moveDuration)
                    .SetEase(Ease.OutCubic);

                _handCards[i].transform
                    .DORotateQuaternion(Quaternion.LookRotation(_handCenter.forward), _moveDuration);
            }
        }
    }
}