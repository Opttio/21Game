using _Project.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _bestButton;

        
        private GameEventBus _gameEventBus;
        [Inject]
        private void Construct(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(SignalToStartGame);
            _bestButton.onClick.AddListener(SignalToBestView);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(SignalToStartGame);
            _bestButton.onClick.RemoveListener(SignalToBestView);
        }

        private void SignalToStartGame()
        {
            _gameEventBus.ChangeView(MyViews.Game);
        }

        private void SignalToBestView() => _gameEventBus.ChangeView(MyViews.BestPlayers);
    }
}