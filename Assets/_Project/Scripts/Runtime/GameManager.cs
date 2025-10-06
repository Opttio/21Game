using _Project.Scripts.Core;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameTurnManager _turnManager;
        [SerializeField] private CardPool _cardPool;
        [SerializeField] private PlayerTurn _player;
        [SerializeField] private OpponentTurn _opponent1;
        [SerializeField] private OpponentTurn _opponent2;
        [SerializeField] private BestPlayersManager _bestPlayersManager;
        
        private int _winStrickPoints = 0;
        
        private GameEventBus _gameEventBus;
        
        [Inject]
        private void Construct(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;
        }
        
        private void OnEnable()
        {
            _gameEventBus.OnRoundFinished += EvaluateRound;
        }

        private void OnDisable()
        {
            _gameEventBus.OnRoundFinished -= EvaluateRound;
        }

        private void Start()
        {
            _gameEventBus.ChangeView(MyViews.Start);
            _gameEventBus.ChangeWinStrickPoints(_winStrickPoints);
            
            var topPlayers = _bestPlayersManager.GetTopPlayers();
            _gameEventBus.ChangeBestPlayers(topPlayers);
        }
        
        private void EvaluateRound()
        {
            int playerPoints = _player.Points;
            int opp1Points = _opponent1.Points;
            int opp2Points = _opponent2.Points;

            bool playerWon = DetermineWinner(playerPoints, opp1Points, opp2Points);

            if (playerWon)
            {
                Debug.Log("🟢 Player wins the round!");
                HandleWin();
            }
            else
            {
                Debug.Log("🔴 Player loses the round!");
                UpdateBestPlayers();
                HandleLoss();
            }
        }
        private bool DetermineWinner(int player, int opp1, int opp2)
        {
            if (player >= 22) return false;
            int bestOpponent = Mathf.Max(opp1 >= 22 ? 0 : opp1, opp2 >= 22 ? 0 : opp2);
            return player >= bestOpponent;
        }

        private void HandleWin()
        {
            ReturnAllCardsToPool();
            _winStrickPoints++;
            _gameEventBus.ChangeWinStrickPoints(_winStrickPoints);
        }

        private void HandleLoss()
        {
            ReturnAllCardsToPool();
            _winStrickPoints = 0;
            _gameEventBus.ChangeWinStrickPoints(_winStrickPoints);
        }

        private void ReturnAllCardsToPool()
        {
            _player.ReturnCardsToPool(_cardPool);
            _opponent1.ReturnCardsToPool(_cardPool);
            _opponent2.ReturnCardsToPool(_cardPool);
        }

        private void UpdateBestPlayers()
        {
            if (_bestPlayersManager.IsTopScore(_winStrickPoints) && _winStrickPoints != 0)
            {
                _gameEventBus.OpenWriteBestPlayerView(_winStrickPoints);
            }
            else
            {
                _gameEventBus.ChangeView(MyViews.GameOver);
            }
        }
    }
}