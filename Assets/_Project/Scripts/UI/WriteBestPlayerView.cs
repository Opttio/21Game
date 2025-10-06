using _Project.Scripts.Core;
using _Project.Scripts.Runtime;
using _Project.Scripts.Structs;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class WriteBestPlayerView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private Button _saveButton;
        
        private int _winStreak;
        
        private GameEventBus _eventBus;
        private BestPlayersManager _bestPlayersManager;

        [Inject]
        public void Construct(GameEventBus eventBus, BestPlayersManager bestPlayersManager)
        {
            _eventBus = eventBus;
            _bestPlayersManager = bestPlayersManager;
        }
        
        public void Init(int winStreak)
        {
            _winStreak = winStreak;
            _nameInput.text = "";
        }
        
        private void OnEnable()
        {
            _eventBus.OnWriteBestPlayerViewOpened += Init;
            _saveButton.onClick.AddListener(OnSaveClicked);
        }

        private void OnDisable()
        {
            _eventBus.OnWriteBestPlayerViewOpened -= Init;
            _saveButton.onClick.RemoveListener(OnSaveClicked);
        }

        private void OnSaveClicked()
        {
            string playerName = _nameInput.text;
            if (string.IsNullOrEmpty(playerName))
                return;

            BestPlayerData newPlayer = new BestPlayerData(playerName, _winStreak);
            _bestPlayersManager.TryAddBestPlayer(newPlayer);

            _eventBus.ChangeView(MyViews.BestPlayers);
        }
    }
}