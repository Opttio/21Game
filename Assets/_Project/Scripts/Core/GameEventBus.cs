using System;
using System.Collections.Generic;
using _Project.Scripts.Structs;
using _Project.Scripts.UI;

namespace _Project.Scripts.Core
{
    public class GameEventBus
    {
        // UI
        public event Action<MyViews> OnViewChanged;
        public event Action<int> OnPointsChanged;
        public event Action<int, string> OnPhraseChanged;
        public event Action<int> OnWinStrickChange;
        public event Action<List<BestPlayerData>> OnBestPlayersChanged;
        public event Action<int> OnWriteBestPlayerViewOpened;

        // Геймплей
        public event Action OnRoundFinished;

        // Виклики
        public void ChangeView(MyViews view) => OnViewChanged?.Invoke(view);
        public void ChangePoints(int points) => OnPointsChanged?.Invoke(points);
        public void ChangePhrase(int opponentId, string phrase) => OnPhraseChanged?.Invoke(opponentId, phrase);
        public void ChangeWinStrickPoints(int winStrickPoints) => OnWinStrickChange?.Invoke(winStrickPoints);
        public void ChangeBestPlayers(List<BestPlayerData> players) => OnBestPlayersChanged?.Invoke(players);
        public void TriggerRoundFinished() => OnRoundFinished?.Invoke();
        
        public void OpenWriteBestPlayerView(int winStreak)
        {
            OnWriteBestPlayerViewOpened?.Invoke(winStreak);
            ChangeView(MyViews.WriteBestPlayer);
        }
    }
}