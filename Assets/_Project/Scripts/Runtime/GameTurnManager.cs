using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class GameTurnManager : MonoBehaviour
    {
        [SerializeField] private PlayerTurn _player;
        [SerializeField] private OpponentTurn _opponent1;
        [SerializeField] private OpponentTurn _opponent2;

        private List<ITurnTaker> _participants = new();
        private bool _gameRunning = true;
        private int _currentIndex = 0;
        
        private GameEventBus _eventBus;

        [Inject]
        private void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Start()
        {
            _participants = new List<ITurnTaker>();
            
            if (_player.gameObject.activeInHierarchy)
                _participants.Add(_player);

            if (_opponent1.gameObject.activeInHierarchy)
                _participants.Add(_opponent1);

            if (_opponent2.gameObject.activeInHierarchy)
                _participants.Add(_opponent2);

            RunTurns().Forget();
        }

        private async UniTaskVoid RunTurns()
        {
            while (_gameRunning)
            {
                var current = _participants[_currentIndex];

                await current.TakeTurn();

                _currentIndex++;

                if (_currentIndex >= _participants.Count)
                {
                    _currentIndex = 0;
                    _eventBus.TriggerRoundFinished();
                }
            }
        }

        public void StopGame()
        {
            _gameRunning = false;
        }
    }
}