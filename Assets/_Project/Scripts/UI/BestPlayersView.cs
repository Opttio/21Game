using System.Collections.Generic;
using _Project.Scripts.Core;
using _Project.Scripts.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class BestPlayersView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _bestPlayersNames;
        [SerializeField] private TextMeshProUGUI[] _bestPlayersScores;
        [SerializeField] private Button _backButton;
        
        private GameEventBus _eventBus;

        [Inject]
        public void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void OnEnable()
        {
            _eventBus.OnBestPlayersChanged += UpdateBestPlayers;
            _backButton.onClick.AddListener(BackToStartView);
        }

        private void OnDisable()
        {
            _eventBus.OnBestPlayersChanged -= UpdateBestPlayers;
            _backButton.onClick.RemoveListener(BackToStartView);
        }

        private void UpdateBestPlayers(List<BestPlayerData> players)
        {
            
            for (int i = 0; i < _bestPlayersNames.Length; i++)
            {
                if (i < players.Count)
                {
                    _bestPlayersNames[i].text = players[i].Name;
                    _bestPlayersScores[i].text = players[i].WinStreak.ToString();
                }
                else
                {
                    _bestPlayersNames[i].text = "-";
                    _bestPlayersScores[i].text = "-";
                }
            }
        }

        private void BackToStartView() => _eventBus.ChangeView(MyViews.Start);
    }
}