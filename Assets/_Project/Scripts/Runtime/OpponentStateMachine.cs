using System;
using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using Random = UnityEngine.Random;
using System.Threading;

namespace _Project.Scripts.Runtime
{
    public class OpponentStateMachine : MonoBehaviour
    {
        public enum State { Waiting, Taking, Pass }

        [SerializeField] private OpponentPhraseSet[] _phraseSets;
        [SerializeField] private int _opponentId;

        private Dictionary<State, string[]> _phrases = new Dictionary<State, string[]>();
        private State _currentState;

        private GameEventBus _eventBus;

        // CancellationToken для керування циклом станів
        private CancellationTokenSource _stateLoopCTS;

        [Inject]
        private void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Start()
        {
            InitializePhrases();
            ChangeState(State.Waiting);
        }

        private void InitializePhrases()
        {
            _phrases.Clear();

            if (_phraseSets != null)
            {
                foreach (var set in _phraseSets)
                {
                    if (set.phrases != null && set.phrases.Length > 0)
                        _phrases[set.state] = set.phrases;
                }
            }
        }

        public void ChangeState(State newState)
        {
            // Зупиняємо попередній цикл стану
            _stateLoopCTS?.Cancel();
            _stateLoopCTS?.Dispose();

            _currentState = newState;
            _stateLoopCTS = new CancellationTokenSource();
            var token = _stateLoopCTS.Token;

            if (newState == State.Pass)
            {
                SpeakPassPhrase(token).Forget();
            }
            else
            {
                SpeakLoopForCurrentState(token).Forget();
            }
        }

        private async UniTask SpeakLoopForCurrentState(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested &&
                       (_currentState == State.Waiting || _currentState == State.Taking))
                {
                    string phrase = GetRandomPhrase(_currentState);
                    _eventBus?.ChangePhrase(_opponentId, phrase);

                    int delay = _currentState == State.Waiting ? Random.Range(10, 15) * 1000 : 2000;
                    await UniTask.Delay(delay, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                // Нормально зупиняємося при Cancel
            }
        }

        private async UniTask SpeakPassPhrase(CancellationToken token)
        {
            try
            {
                string phrase = GetRandomPhrase(State.Pass);
                _eventBus?.ChangePhrase(_opponentId, phrase);

                await UniTask.Delay(3000, cancellationToken: token);

                // Повертаємося в стан Waiting після Pass
                if (!token.IsCancellationRequested)
                    ChangeState(State.Waiting);
            }
            catch (OperationCanceledException)
            {
                // Cancel – нічого не робимо
            }
        }

        private string GetRandomPhrase(State state)
        {
            if (_phrases == null || !_phrases.ContainsKey(state) || _phrases[state] == null || _phrases[state].Length == 0)
                return "...";

            var list = _phrases[state];
            return list[Random.Range(0, list.Length)];
        }
    }

    [Serializable]
    public struct OpponentPhraseSet
    {
        public OpponentStateMachine.State state;
        public string[] phrases;
    }
}