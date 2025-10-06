using _Project.Scripts.Core;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private Canvas[] _views;
        
        private GameEventBus _eventBus;

        [Inject]
        private void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void OnEnable()
        {
            _eventBus.OnViewChanged += OnViewChanged;
        }

        private void OnDisable()
        {
            _eventBus.OnViewChanged -= OnViewChanged;
        }

        private void OnViewChanged(MyViews view)
        {
            ActivateView(view);
        }

        public void ActivateView(MyViews view)
        {
            int id = (int)view;
            if (id < 0 || id >= _views.Length)
                return;

            foreach (var v in _views)
                v.enabled = false;

            _views[id].enabled = true;
        }
    }

    public enum MyViews
    {
        Start = 0,
        Game = 1,
        GameOver = 2,
        WriteBestPlayer = 3,
        BestPlayers = 4
    }
}