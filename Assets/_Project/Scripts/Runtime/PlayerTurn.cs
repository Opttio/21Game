using _Project.Scripts.UI;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class PlayerTurn : MonoBehaviour, ITurnTaker
    {
        [SerializeField] private CardHandLayout _handLayout;
        public string Name => "Player";
        public int Points => _handLayout.TotalPoints;

        private CardPool _cardPool;

        private GameView _gameView;
        private bool _turnFinished = false;

        [Inject]
        public void Construct(CardPool cardPool, GameView gameView)
        {
            _cardPool = cardPool;
            _gameView = gameView;
        }

        public async UniTask TakeTurn()
        {
            _turnFinished = false;
            _gameView.SetTurnListeners(TakeCard, Pass);

            while (!_turnFinished)
            {
                await UniTask.Yield();
            }
            
            _gameView.ClearTurnListeners();
        }
        
        public void ReturnCardsToPool(CardPool pool)
        {
            _handLayout.ReturnAllToPool(pool);
        }

        private void TakeCard()
        {
            var card = _cardPool.GetNextCard();
            if (card)
                _handLayout.AddCard(card);
        }

        private void Pass()
        {
            _turnFinished = true;
        }
    }
}