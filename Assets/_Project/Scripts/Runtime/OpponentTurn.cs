using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Runtime
{
    public class OpponentTurn : MonoBehaviour, ITurnTaker
    {
        [SerializeField] private CardHandLayout _handLayout;
        [SerializeField] private OpponentStateMachine _opponentStateMachine;
        
        public int Points => _handLayout.TotalPoints;

        public string Name => gameObject.name;

        private CardPool _cardPool;


        [Inject]
        public void Construct(CardPool cardPool)
        {
            _cardPool = cardPool;
        }

        public async UniTask TakeTurn()
        {
            _opponentStateMachine.ChangeState(OpponentStateMachine.State.Taking);
            bool turnFinished = false;

            while (!turnFinished)
            {
                await UniTask.Delay(2000);
                int currentPoints = _handLayout.TotalPoints;
                if (currentPoints < 17)
                    DrawCard();
                
                else if (currentPoints >= 17 && currentPoints <= 19)
                {
                    if (Random.value < 0.5f)
                        DrawCard();
                    else
                        turnFinished = true;
                }
                else
                {
                    turnFinished = true;
                }
            }
            _opponentStateMachine.ChangeState(OpponentStateMachine.State.Pass);
        }
        
        public void ReturnCardsToPool(CardPool pool)
        {
            _handLayout.ReturnAllToPool(pool);
        }

        private void DrawCard()
        {
            var card = _cardPool.GetNextCard();
            if (card)
            {
                _handLayout.AddCard(card);
                Debug.Log($"{Name} drew a card. Total points: {_handLayout.TotalPoints}");
            }
            else
            {
                Debug.LogWarning($"{Name} tried to draw a card but deck is empty!");
            }
        }
    }
}