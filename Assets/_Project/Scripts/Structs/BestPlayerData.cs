using System;

namespace _Project.Scripts.Structs
{
    [Serializable]
    public struct BestPlayerData
    {
        public string Name;
        public int WinStreak;

        public BestPlayerData(string name, int winStreak)
        {
            Name = name;
            WinStreak = winStreak;
        }
    }
}