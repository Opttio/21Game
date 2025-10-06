using System;
using System.Collections.Generic;
using _Project.Scripts.Structs;
using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class PrefsBestPlayersStorage : IBestPlayersStorage
    {
        private const string Key = "BestPlayers";

        public List<BestPlayerData> Load()
        {
            if (!PlayerPrefs.HasKey(Key))
                return new List<BestPlayerData>();
            string json = PlayerPrefs.GetString(Key);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            return wrapper.Players ?? new List<BestPlayerData>();
        }

        public void Save(List<BestPlayerData> players)
        {
            Wrapper wrapper = new Wrapper { Players = players };
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }
        
        [Serializable]
        private class Wrapper
        {
            public List<BestPlayerData> Players;
        }
    }
}