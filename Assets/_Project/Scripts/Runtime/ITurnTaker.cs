using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Runtime
{
    public interface ITurnTaker
    {
        string Name { get; }
        UniTask TakeTurn();
    }
}