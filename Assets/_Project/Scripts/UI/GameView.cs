using _Project.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _points;
        [SerializeField] private TextMeshProUGUI[] _opponentBars;
        [SerializeField] private Button _takeButton;
        [SerializeField] private Button _passButton;
        [SerializeField] private TextMeshProUGUI _winStrickPoints;
        
        private System.Action _onTake;
        private System.Action _onPass;
        
        private GameEventBus _eventBus;

        [Inject]
        public void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        
        private void OnEnable() => SubscribeToEvents();

        private void OnDisable() => UnsubscribeFromEvents();
        
        private void OnDestroy() => ClearTurnListeners();

        private void SubscribeToEvents()
        {
            _eventBus.OnPointsChanged += UpdatePoints;
            _eventBus.OnPhraseChanged += UpdateBar;
            _eventBus.OnWinStrickChange += UpdateWinStrickPoints;
        }

        private void UnsubscribeFromEvents()
        {
            _eventBus.OnPointsChanged -= UpdatePoints;
            _eventBus.OnPhraseChanged -= UpdateBar;
            _eventBus.OnWinStrickChange -= UpdateWinStrickPoints;
        }

        private void UpdatePoints(int points)
        {
            _points.text = points.ToString();
        }

        private void UpdateBar(int opponentId, string phrase)
        {
            if (opponentId >= 0 && opponentId < _opponentBars.Length)
                _opponentBars[opponentId].text = phrase;
        }

        private void UpdateWinStrickPoints(int winStrickPoints)
        {
            _winStrickPoints.text = winStrickPoints.ToString();
        }
        
        public void SetTurnListeners(System.Action onTake, System.Action onPass)
        {
            _onTake = onTake;
            _onPass = onPass;

            if (_takeButton)
            {
                _takeButton.gameObject.SetActive(true);
                _takeButton.onClick.RemoveAllListeners();
                _takeButton.onClick.AddListener(() => _onTake?.Invoke());
            }

            if (_passButton)
            {
                _passButton.gameObject.SetActive(true);
                _passButton.onClick.RemoveAllListeners();
                _passButton.onClick.AddListener(() => _onPass?.Invoke());
            }
        }

        public void ClearTurnListeners()
        {
            _onTake = null;
            _onPass = null;

            _takeButton.gameObject.SetActive(false);
            _passButton.gameObject.SetActive(false);

            if (_takeButton)
                _takeButton.onClick.RemoveAllListeners();

            if (_passButton)
                _passButton.onClick.RemoveAllListeners();
        }
    }
}