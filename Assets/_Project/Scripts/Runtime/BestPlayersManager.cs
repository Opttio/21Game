using System.Collections.Generic;
using _Project.Scripts.Core;
using _Project.Scripts.Structs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class BestPlayersManager : MonoBehaviour
    {
        private IBestPlayersStorage _storage;
        private List<BestPlayerData> _bestPlayers;
        private GameEventBus _eventBus;
        
        [Inject]
        private void Construct(IBestPlayersStorage storage, GameEventBus eventBus)
        {
            _storage = storage;
            _eventBus = eventBus;
        }
        
        private void Awake()
        {
            _bestPlayers = _storage.Load();
        }
        
        public bool IsTopScore(int winStreak)
        {
            if (_bestPlayers.Count < 5)
                return true;
            int minWinStreak = int.MaxValue;
            foreach (var player in _bestPlayers)
            {
                if (player.WinStreak < minWinStreak)
                    minWinStreak = player.WinStreak;
            }

            return winStreak > minWinStreak;
        }
        
        public void TryAddBestPlayer(BestPlayerData newPlayer)
        {
            if (_bestPlayers.Count < 5)
                _bestPlayers.Add(newPlayer);
            else
            {
                int minIndex = 0;
                int minWinStreak = _bestPlayers[0].WinStreak;
                for (int i = 1; i < _bestPlayers.Count; i++)
                {
                    if (_bestPlayers[i].WinStreak < minWinStreak)
                    {
                        minWinStreak = _bestPlayers[i].WinStreak;
                        minIndex = i;
                    }
                }

                if (newPlayer.WinStreak > minWinStreak)
                    _bestPlayers[minIndex] = newPlayer;
                else
                    return;
            }

            // Сортую список від більшого до меншого WinStreak
            _bestPlayers.Sort((a, b) => b.WinStreak.CompareTo(a.WinStreak));

            _storage.Save(_bestPlayers);
            _eventBus.ChangeBestPlayers(_bestPlayers);
        }
        
        public List<BestPlayerData> GetTopPlayers()
        {
            int count = Mathf.Min(_bestPlayers.Count, 5);
            return _bestPlayers.GetRange(0, count);
        }
    }
}