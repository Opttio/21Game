using System.Collections.Generic;
using _Project.Scripts.Structs;

namespace _Project.Scripts.Runtime
{
    public interface IBestPlayersStorage
    {
        List<BestPlayerData> Load();
        void Save(List<BestPlayerData> players);
    }
}