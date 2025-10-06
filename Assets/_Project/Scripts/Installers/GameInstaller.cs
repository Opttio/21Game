using _Project.Scripts.Core;
using _Project.Scripts.Runtime;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CardPool _cardPool;
        [SerializeField] private ViewManager _viewManager;

        [Header("UI & Opponents")]
        [SerializeField] private GameView _gameView;

        public override void InstallBindings()
        {
            // Прив'язка конкретних інстансів через інспектор
            Container.Bind<CardPool>().FromInstance(_cardPool).AsSingle();
            Container.Bind<ViewManager>().FromInstance(_viewManager).AsSingle();

            // MonoBehaviour на сцені автоматично підхоплюється
            Container.Bind<BestPlayersManager>().FromComponentInHierarchy().AsSingle();

            // Глобальні сервіси
            Container.Bind<GameEventBus>().AsSingle();
            Container.Bind<IBestPlayersStorage>().To<PrefsBestPlayersStorage>().AsSingle();

            // Інші UI-компоненти
            Container.BindInstance(_gameView).AsSingle();
        }
    }
}